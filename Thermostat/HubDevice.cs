namespace HomeHub.Hub
{
    using HomeHub.Shared;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [DataContract]
    class HubDevice : IDevice
    {
        private static List<Type> _supportedSensorReadings = new List<Type>() { typeof(TemperatureReading), typeof(MotionReading) };
        private static Dictionary<DeviceFunction, bool> _supportedCommandFunctions = new Dictionary<DeviceFunction, bool>()
        {
            { DeviceFunction.Heat, false },
            { DeviceFunction.Fan, false }
        };

        public HubDevice()
        {
            Id = "Hub";
            FriendlyName = "Hub";

            // TODO: Initialize
            // TODO: Pull pin values from settings? Or is it better to pass them in as parameters?
            // TODO: Track Furnace/Fan hours for filter?
        }

        [DataMember]
        public string FriendlyName
        {
            get;
            set;
        }

        [DataMember]
        public string Id
        {
            get;
            private set;
        }

        [DataMember]
        public bool IsOnline
        {
            get
            {
                return true;
            }
        }

        [DataMember]
        public DateTime LastHeartBeat
        {
            get;
            private set;
        }

        [DataMember]
        public IReadOnlyDictionary<DeviceFunction,bool> SupportedCommandFunctions
        {
            get
            {
                return _supportedCommandFunctions;
            }
        }

        [DataMember]
        public IEnumerable<Type> SupportedSensorReadings
        {
            get
            {
                return _supportedSensorReadings;
            }
        }

        public Task<bool> SendCommands(IEnumerable<DeviceCommand> commands)
        {
            var newTask = Task.Run<bool>(() =>
            {
                bool completed = true;

                foreach (var command in commands)
                {
                    if (command.Function == DeviceFunction.Heat)
                    {
                        // TODO: Turn on furnace
                        ActivateFurnace(command.ShouldActivate);
                        completed = completed && true;
                    }
                    else if (command.Function == DeviceFunction.Fan)
                    {
                        // TODO: Turn on fan
                        ActivateFan(command.ShouldActivate);
                        completed = completed && true;
                    }
                    else if (command.Function == DeviceFunction.Cool)
                    {
                        throw new NotSupportedException();
                    }
                }

                LastHeartBeat = DateTime.Now;

                return completed;
            });

            return newTask;
        }

        public Task<IEnumerable<ISensorReading>> TakeReadings()
        {
            Task<IEnumerable<ISensorReading>> newTask = Task.Run<IEnumerable<ISensorReading>>(() =>
            {
                List<ISensorReading> readings = new List<ISensorReading>();
                
                readings.Add(new TemperatureReading(Id, ReadThermometer()));
                readings.Add(new MotionReading(Id, ReadMotionSensor()));

                LastHeartBeat = DateTime.Now;

                return readings;
            });

            return newTask;
        }

        private Temperature ReadThermometer()
        {
            return new Temperature(TemperatureScale.Fahrenheit, 68);
        }

        private bool ReadMotionSensor()
        {
            return false;
        }

        private void ActivateFurnace(bool shouldActivate)
        {
            _supportedCommandFunctions[DeviceFunction.Heat] = true;
        }

        private void ActivateFan(bool shouldActivate)
        {
            _supportedCommandFunctions[DeviceFunction.Fan] = true;
        }
    }
}
