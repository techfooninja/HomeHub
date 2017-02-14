namespace HomeHub.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;

    public class ThermostatViewModel : NotificationBase<ThermostatProxy>
    {
        ObservableCollection<RuleViewModel> _rules = new ObservableCollection<RuleViewModel>();
        Temperature _currentTemperature = new Temperature();

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
                    ReloadRules(value.Rules);
                    RaisePropertyChanged("CurrentTemperature");
                }
            }
        }

        public Temperature CurrentTemperature
        {
            get
            {
                return (This.CurrentAverageTemperature ?? new Temperature()).ConvertToScale(ClientSettings.TemperatureFormat);
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
                rvm.PropertyChanged += Rule_PropertyChanged;
                _rules.Add(rvm);
            }

            RaisePropertyChanged("Rules");
        }

        private void Rule_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
    }
}
