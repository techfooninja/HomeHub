namespace HomeHub.Client.ViewModels
{
    using HomeHub.Shared;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;

    public class ThermostatViewModel : NotificationBase<ThermostatProxy>
    {
        ObservableCollection<RuleViewModel> _rules = new ObservableCollection<RuleViewModel>();
        ObservableCollection<DeviceViewModel> _devices = new ObservableCollection<DeviceViewModel>();
        bool hasFailedUpdate = false;

        public ThermostatViewModel(ThermostatProxy proxy = null) : base(proxy)
        {
            ClientSettingsViewModel.Instance.PropertyChanged += TemperatureFormat_PropertyChanged;
        }

        private void TemperatureFormat_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedTemperatureScale")
            {
                RaisePropertyChanged("CurrentTemperature");
                RaisePropertyChanged("CurrentTemperatureUnit");
            }
        }

        public ThermostatProxy Thermostat
        {
            get { return This; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref This, value);
                    ReloadRules(This.Rules);
                    ReloadDevices(This.Devices);
                    RaisePropertyChanged("CurrentTemperature");
                }
            }
        }

        public bool IsAvailable
        {
            get
            {
                return !hasFailedUpdate;
            }
        }

        public Temperature CurrentTemperature
        {
            get
            {
                return This.CurrentAverageTemperature != null
                    ? This.CurrentAverageTemperature.ConvertToScale(ClientSettings.TemperatureFormat)
                    : null;
            }
        }

        public string CurrentTemperatureUnit
        {
            get { return ClientSettingsViewModel.Instance.TemperatureFormatUnit; }
        }

        public ObservableCollection<RuleViewModel> Rules
        {
            get { return _rules; }
            set { SetProperty(ref _rules, value); }
        }

        private void ReloadRules(IEnumerable<Rule> rules)
        {
            _rules.Clear();

            foreach (var rule in rules)
            {
                var rvm = new RuleViewModel(rule);
                rvm.PropertyChanged += Rule_PropertyChanged; // TODO: Necessary?

                if (rule.Id == This.CurrentRuleId)
                {
                    rvm.IsCurrent = true;
                }

                _rules.Add(rvm);
            }

            RaisePropertyChanged("Rules");
        }

        public ObservableCollection<DeviceViewModel> Devices
        {
            get { return _devices; }
            set { SetProperty(ref _devices, value); }
        }

        private void ReloadDevices(IEnumerable<Device> devices)
        {
            _devices.Clear();

            foreach (var device in devices)
            {
                var dvm = new DeviceViewModel(device);
                _devices.Add(dvm);
            }

            RaisePropertyChanged("Devices");
        }

        private void Rule_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Rule")
                RaisePropertyChanged("Rules");

            if (e.PropertyName == "Deleted")
            {
                _rules.Remove((RuleViewModel)sender);
                RaisePropertyChanged("Rules");
            }
        }

        public async Task GetUpdates()
        {
            try
            {
                Thermostat = await ThermostatProxy.GetUpdates();
                hasFailedUpdate = false;
            }
            catch
            {
                if (!hasFailedUpdate)
                {
                    hasFailedUpdate = true;
                    await ShowPopUp("Hub unreachable");
                }
            }

            RaisePropertyChanged("IsAvailable");
        }
    }
}
