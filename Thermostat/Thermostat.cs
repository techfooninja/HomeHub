namespace HomeHub.Hub
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using HomeHub.Shared;

    [DataContract]
    class Thermostat : IThermostat
    {
        private static Thermostat _instance = null;
        private static RuleComparer _ruleComparer = new RuleComparer();

        private Timer _pollingTimer;
        private List<IDevice> _devices;
        private List<Rule> _rules;
        private TemperatureState _previousTempState;
        private TimeSpan _targetBufferTime;

        private Thermostat()
        {
            _devices = new List<IDevice>();

            // Add Hub Device, which should always be present since it represents this device
            _devices.Add(new HubDevice());

            // TODO: Add other devices here

            _rules = new List<Rule>();

            // Add Default Rule, which should always be there
            _rules.Add(new DefaultRule());

            // TODO: Add other rules here

            // Configure default state to be target. Sensors will need to be polled to take any action
            _previousTempState = TemperatureState.Target;

            // Configure target buffer time, default to 2 minutes
            TargetBufferTime = 120;

            // Set up timer
            _pollingTimer = new Timer(TimerCallback, null, 0, PollingTime * 1000);
        }
        
        [IgnoreDataMember]
        public static Thermostat Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Thermostat();
                }
                return _instance;
            }
        }

        [DataMember]
        public int PollingTime
        {
            get
            {
                return SettingsHelper.GetProperty<int>(60);
            }
            set
            {
                SettingsHelper.SetProperty(value);
                _pollingTimer.Change(1000, value * 1000);
            }
        }

        [DataMember]
        public int TargetBufferTime
        {
            get
            {
                return SettingsHelper.GetProperty<int>(120);
            }
            set
            {
                SettingsHelper.SetProperty(value);
            }
        }

        [DataMember]
        public bool UseRules
        {
            get
            {
                return SettingsHelper.GetProperty<bool>(true);
            }
            set
            {
                SettingsHelper.SetProperty(value);
            }
        }

        [DataMember]
        public IEnumerable<IDevice> Devices
        {
            get { return _devices; }
        }

        [DataMember]
        public IEnumerable<Rule> Rules
        {
            get
            {
                // TODO: Find a way to avoid sorting on every property read
                //       SortedSet does not guarentee sorted order when a sort-property is changed after the object is added
                _rules.Sort(_ruleComparer);
                return _rules;
            }
        }

        [DataMember]
        public Temperature CurrentAverageTemperature { get; private set; }

        [DataMember]
        public IEnumerable<TemperatureReading> CurrentTemperatures { get; private set; }

        public void AddRule(Rule newRule)
        {
            _rules.Add(newRule);
        }

        public bool DeleteRule(string id)
        {
            return _rules.RemoveAll(r => r.Id == id) > 0;
        }

        public void UpdateRule(Rule rule)
        {
            DeleteRule(rule.Id);
            AddRule(rule);
        }

        private async void TimerCallback(object state)
        {
            // Clean out old rules
            _rules.RemoveAll(r => r is TemporaryOverrideRule ? ((TemporaryOverrideRule)r).Expiration < DateTime.Now : false);

            List<Task<IEnumerable<ISensorReading>>> tasks = new List<Task<IEnumerable<ISensorReading>>>();
            List<ISensorReading> readings = new List<ISensorReading>();

            // Start taking readings from each device to maximize parallelism
            foreach (var device in _devices)
            {
                tasks.Add(device.TakeReadings());
            }

            // Wait for each task to complete
            foreach (var task in tasks)
            {
                readings.AddRange(await task);
            }

            // Store temperature readings
            CurrentTemperatures = readings.OfType<TemperatureReading>();
            CurrentAverageTemperature = Temperature.Average(CurrentTemperatures.Select(tr => tr.Temperature));

            // Get Temperature State from the current rule set
            TemperatureState currentTempState = Rules.First(r => r.IsApplicableNow()).ProcessReadings(readings);

            // Handle temperature state
            await HandleTemperatureState(currentTempState);
        }

        private Task HandleTemperatureState(TemperatureState tempState)
        {
            Task newTask = Task.Run(() =>
            {
                if (tempState == _previousTempState && _targetBufferTime == null)
                {
                    // Temperature state hasn't changed and we're not on buffer time, so don't do anything
                    return;
                }

                List<DeviceCommand> commands = new List<DeviceCommand>();

                if (tempState == TemperatureState.Low)
                {
                    // Temperature is low, turn on devices that control heat
                    commands.Add(new DeviceCommand() { Function = DeviceFunction.Heat, ShouldActivate = true });

                    if (_previousTempState == TemperatureState.High)
                    {
                        // Turn off devices that cool
                        commands.Add(new DeviceCommand() { Function = DeviceFunction.Fan, ShouldActivate = false });
                        // TODO: Add Cool here when it is supported
                    }
                }

                if (tempState == TemperatureState.High)
                {
                    // Temperature is high, turn on devices that control cooling
                    commands.Add(new DeviceCommand() { Function = DeviceFunction.Fan, ShouldActivate = true });

                    if (_previousTempState == TemperatureState.Low)
                    {
                        // Turn off devices that heat
                        commands.Add(new DeviceCommand() { Function = DeviceFunction.Heat, ShouldActivate = false });
                    }
                }

                if (tempState == TemperatureState.Target)
                {
                    // Temperature is at target, wait the buffer time before turning devices off
                    // TODO: Turn fan on for a few minutes after heat to distribute heat throughout the house?
                    if (_targetBufferTime == null)
                    {
                        _targetBufferTime = DateTime.Now.AddSeconds(TargetBufferTime).TimeOfDay;
                    }
                    else
                    {
                        if (DateTime.Now.TimeOfDay >= _targetBufferTime)
                        {
                            commands.Add(new DeviceCommand() { Function = DeviceFunction.Heat, ShouldActivate = false });
                            commands.Add(new DeviceCommand() { Function = DeviceFunction.Fan, ShouldActivate = false });
                            // TODO: Add cooling when it is supported
                        }
                    }
                }

                var eligibleDevices = _devices.Where(d => d.SupportedCommandFunctions.Keys.Intersect(commands.Select(c => c.Function)).Count() > 0);
                foreach (var device in eligibleDevices)
                {
                    device.SendCommands(commands);
                }

                _previousTempState = tempState;
            });

            return newTask;
        }
    }
}
