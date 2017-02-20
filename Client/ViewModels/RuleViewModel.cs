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

        public RuleViewModel(Rule rule = null) : base(rule)
        {
            ClientSettingsViewModel.Instance.PropertyChanged += TemperatureFormat_PropertyChanged;
            LowTemperature = This.LowTemperature ?? new Temperature();
            HighTemperature = This.HighTemperature ?? new Temperature();

            if (rule != null && Rule.GetTypeById(rule.Id) == typeof(TemporaryOverrideRule))
            {
                _isOverride = true;
            }
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
                SetProperty(This.IsEnabled, value, () => This.IsEnabled = value);
            }
        }

        public TimeSpan StartTime
        {
            get { return This.StartTime; }
            set
            {
                SetProperty(This.StartTime, value, () => This.StartTime = value);
            }
        }

        public TimeSpan EndTime
        {
            get { return This.EndTime; }
            set
            {
                SetProperty(This.EndTime, value, () => This.EndTime = value);
            }
        }

        public bool HasTimes
        {
            get
            {
                return RuleType == typeof(ScheduleRule);
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
            get { return This.Expiration.Date; }
            set
            {
                SetProperty(This.Expiration.Date, value, () => This.Expiration = value.Add(ExpirationTime).DateTime);
                RaisePropertyChanged("ExpirationDateTime");
            }
        }

        public TimeSpan ExpirationTime
        {
            get { return This.Expiration.TimeOfDay; }
            set
            {
                SetProperty(This.Expiration.TimeOfDay, value, () => This.Expiration = ExpirationDate.Add(value).DateTime);
                RaisePropertyChanged("ExpirationDateTime");
            }
        }

        public DateTimeOffset ExpirationDateTime
        {
            get { return This.Expiration; }
        }

        public bool CanExpire
        {
            get
            {
                return RuleType == typeof(TemporaryOverrideRule);
            }
        }

        public Type RuleType
        {
            get
            {
                return Rule.GetTypeById(This.Id);
            }
        }

        public bool CanDelete
        {
            get
            {
                return RuleType != typeof(DefaultRule);
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

        public async Task Save()
        {
            ProgressViewModel progress = ProgressViewModel.Instance;

            progress.BlockingProgressText = "Saving changes";
            progress.IsBlockingProgress = true;

            if (IsOverride)
            {
                var response = await ThermostatProxy.SetHoldTemp(This, ExpirationDateTime.DateTime);
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

            RaisePropertyChanged("Rule");
        }

        public async Task Delete()
        {
            ProgressViewModel progress = ProgressViewModel.Instance;

            progress.BlockingProgressText = "Deleting rule";
            progress.IsBlockingProgress = true;

            var response = await ThermostatProxy.DeleteRule(This);
            // TODO: Throw exception when it fails

            progress.IsBlockingProgress = false;

            RaisePropertyChanged("Deleted");
        }
    }
}
