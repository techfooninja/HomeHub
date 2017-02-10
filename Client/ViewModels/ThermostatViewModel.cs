namespace HomeHub.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;

    public class ThermostatViewModel : NotificationBase
    {
        ObservableCollection<RuleViewModel> _rules = new ObservableCollection<RuleViewModel>();
        Temperature _currentTemperature = new Temperature();

        public ThermostatViewModel()
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

        public Temperature CurrentTemperature
        {
            get { return _currentTemperature.ConvertToScale(ClientSettings.TemperatureFormat); }
            set { SetProperty(ref _currentTemperature, value.ConvertToScale(ClientSettings.TemperatureFormat)); }
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

        public void ReloadRules(IEnumerable<Rule> rules)
        {
            _rules.Clear();

            foreach (var rule in rules)
            {
                var rvm = new RuleViewModel(rule);
                rvm.PropertyChanged += Rule_PropertyChanged;
                _rules.Add(rvm);
            }
        }

        private void Rule_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }
    }
}
