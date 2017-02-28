namespace HomeHub.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Windows.Storage.Pickers;

    public class HubSettingsViewModel : NotificationBase<HubSettings>
    {
        public HubSettingsViewModel(HubSettings settings = null) : base(settings)
        {

        }

        public int PollingTime
        {
            get { return This.PollingTime; }
            set { SetProperty(This.PollingTime, value, () => This.PollingTime = value); }
        }

        public int TargetBufferTime
        {
            get { return This.TargetBufferTime; }
            set { SetProperty(This.TargetBufferTime, value, () => This.TargetBufferTime = value); }
        }

        public bool UseRules
        {
            get { return This.UseRules; }
            set { SetProperty(This.UseRules, value, () => This.UseRules = value); }
        }

        public void UpdateSettings(ThermostatViewModel thermostat)
        {
            This.UpdateSettings(thermostat.Thermostat);
            RaisePropertyChanged("PollingTime");
            RaisePropertyChanged("TargetBufferTime");
            RaisePropertyChanged("UseRules");
        }

        public async void Export()
        {
            var picker = new FileSavePicker();
            picker.SuggestedStartLocation = PickerLocationId.Downloads;
            picker.SuggestedFileName = String.Format("{0:yyyy}-{0:MM}-{0:dd}.{1}", DateTime.Now, "hubconfig");
            picker.DefaultFileExtension = ".hubconfig";
            picker.FileTypeChoices.Add("Hub Configuration", new List<string>() { ".hubconfig" });
            var file = await picker.PickSaveFileAsync();

            if (file != null)
            {
                bool success = await This.Export(file);
                if (!success)
                {
                    await ShowPopUp("An error occurred while exporting");
                }
            }
        }

        public async void Import()
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Downloads;
            picker.ViewMode = PickerViewMode.List;
            picker.FileTypeFilter.Add(".hubconfig");

            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                bool success = await This.Import(file);
                if (!success)
                {
                    await ShowPopUp("An error occurred while importing");
                }
            }
        }
    }
}
