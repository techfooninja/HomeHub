namespace HomeHub.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;
    using Windows.UI.Xaml.Controls;
    using System.ComponentModel;

    public class RuleViewModel : NotificationBase<Rule>
    {
        private bool _isNewRule;
        private bool _isOverride;
        private TimeSpan _expirationTime;
        private DateTimeOffset _expirationDate;

        public RuleViewModel(Rule rule = null) : base(rule)
        {
            ClientSettingsViewModel.Instance.PropertyChanged += TemperatureFormat_PropertyChanged;
            LowTemperature = This.LowTemperature ?? new Temperature();
            HighTemperature = This.HighTemperature ?? new Temperature();
        }

        private void TemperatureFormat_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedTemperatureScale")
            {
                RaisePropertyChanged("LowTemperature");
                RaisePropertyChanged("HighTemperature");
                RaisePropertyChanged("CurrentTemperatureUnit");
            }
        }

        public Temperature LowTemperature
        {
            get { return This.LowTemperature.ConvertToScale(ClientSettings.TemperatureFormat); }
            set { SetProperty(This.LowTemperature, value, () => This.LowTemperature = value); }
        }

        public Temperature HighTemperature
        {
            get { return This.HighTemperature.ConvertToScale(ClientSettings.TemperatureFormat); }
            set { SetProperty(This.HighTemperature, value, () => This.HighTemperature = value); }
        }

        public string CurrentTemperatureUnit
        {
            get { return ClientSettingsViewModel.Instance.TemperatureFormatUnit; }
        }

        public bool IsEnabled
        {
            get { return This.IsEnabled; }
            set
            {
                if (RuleType == typeof(ScheduleRule))
                {
                    SetProperty(This.IsEnabled, value, () => This.IsEnabled = value);
                }
            }
        }

        public TimeSpan StartTime
        {
            get { return This.StartTime; }
            set
            {
                if (RuleType == typeof(ScheduleRule))
                {
                    SetProperty(This.StartTime, value, () => This.StartTime = value);
                }
            }
        }

        public TimeSpan EndTime
        {
            get { return This.EndTime; }
            set
            {
                if (RuleType == typeof(ScheduleRule))
                {
                    SetProperty(This.EndTime, value, () => This.EndTime = value);
                }
            }
        }

        public bool IsNewRule
        {
            get { return _isNewRule; }
            set
            {
                SetProperty(_isNewRule, value, () => _isNewRule = value);
            }
        }

        public bool IsOverride
        {
            get { return _isOverride; }
            set
            {
                SetProperty(_isOverride, value, () => _isOverride = value);
            }
        }

        public DateTimeOffset ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                SetProperty(_expirationDate, value, () => _expirationDate = value);
            }
        }

        public TimeSpan ExpirationTime
        {
            get { return _expirationTime; }
            set
            {
                SetProperty(_expirationTime, value, () => _expirationTime = value);
            }
        }

        public Type RuleType
        {
            get
            {
                return Rule.GetTypeById(This.Id);
            }
        }

        public Symbol RuleSymbol
        {
            get
            {
                if (RuleType == typeof(DefaultRule))
                    return Symbol.Pin;
                else if (RuleType == typeof(TemporaryOverrideRule))
                    return Symbol.ReportHacked;
                else if (RuleType == typeof(ScheduleRule))
                    return Symbol.Clock;
                else
                    return Symbol.Placeholder;
            }
        }

        public async void Save()
        {
            ProgressViewModel progress = ProgressViewModel.Instance;

            progress.BlockingProgressText = "Saving changes";
            progress.IsBlockingProgress = true;

            if (IsOverride)
            {
                var response = await ThermostatProxy.SetHoldTemp(This, ExpirationDate.DateTime.Add(ExpirationTime));
                // TODO: Throw exception when it fails
            }
            else
            {
                if (IsNewRule)
                {
                    var response = await ThermostatProxy.AddRule(This);
                    // TODO: Throw exception when it fails
                }
                else
                {
                    var response = await ThermostatProxy.UpdateRule(This);
                    // TODO: Throw exception when it fails
                }
            }

            progress.IsBlockingProgress = false;
        }
    }
}
