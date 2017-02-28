namespace HomeHub.Client.ViewModels
{
    using HomeHub.Shared;
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Controls;
    using Windows.Web.Http;

    public class RuleViewModel : NotificationBase<Rule>
    {
        private bool _isNewRule;
        private bool _isOverride;
        private bool _isCurrent;

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

        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                SetProperty(ref _isCurrent, value);
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

        public bool CanDisable
        {
            get
            {
                return RuleType != typeof(DefaultRule);
            }
        }

        public async Task Save()
        {
            ProgressViewModel progress = ProgressViewModel.Instance;

            progress.BlockingProgressText = "Saving changes";
            progress.IsBlockingProgress = true;

            HttpResponseMessage response = null;

            try
            {
                if (IsOverride)
                {
                    response = await ThermostatProxy.SetHoldTemp(This, ExpirationDateTime.DateTime);
                }
                else
                {
                    if (IsNewRule)
                    {
                        response = await ThermostatProxy.AddRule(This);
                    }
                    else
                    {
                        response = await ThermostatProxy.UpdateRule(This);
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    await ShowPopUp(String.Format("{0} - {1}", response.StatusCode, response.ReasonPhrase), "Update Failed");
                }

                RaisePropertyChanged("Rule");
            }
            catch
            {
                await ShowPopUp("Hub unreachable");
            }
            finally
            {
                progress.IsBlockingProgress = false;
            }
        }

        public async Task ToggleIsEnabled()
        {
            IsEnabled = !IsEnabled;
            await Save();
        }

        public async Task Delete()
        {
            ProgressViewModel progress = ProgressViewModel.Instance;

            progress.BlockingProgressText = "Deleting rule";
            progress.IsBlockingProgress = true;

            try
            {
                var response = await ThermostatProxy.DeleteRule(This);

                if (!response.IsSuccessStatusCode)
                {
                    await ShowPopUp(String.Format("{0} - {1}", response.StatusCode, response.ReasonPhrase), "Delete Failed");
                }

                RaisePropertyChanged("Deleted");
            }
            catch
            {
                await ShowPopUp("Hub unreachable");
            }
            finally
            {
                progress.IsBlockingProgress = false;
            }
        }
    }
}
