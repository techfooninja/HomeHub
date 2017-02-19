namespace HomeHub.Client.ViewModels
{
    using Shared;

    public class ClientSettingsViewModel : NotificationBase
    {
        private static ClientSettingsViewModel _instance = null;

        private int _selectedIndex;

        private ClientSettingsViewModel()
        {
            SelectedIndex = (int)ClientSettings.TemperatureFormat;
        }

        public static ClientSettingsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ClientSettingsViewModel();
                }

                return _instance;
            }
        }

        public string[] PossibleTemperatureFormats
        {
            get { return ClientSettings.PossibleTemperatureFormats; }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }

            set
            {
                if (SetProperty(ref _selectedIndex, value))
                {
                    ClientSettings.TemperatureFormat = (TemperatureScale)value;
                    RaisePropertyChanged(nameof(SelectedTemperatureScale));
                }
            }
        }

        public TemperatureScale SelectedTemperatureScale
        {
            get { return (TemperatureScale)SelectedIndex; }
        }

        public string TemperatureFormatUnit
        {
            get
            {
                switch (SelectedTemperatureScale)
                {
                    case TemperatureScale.Celsius:
                        return "°C";
                    case TemperatureScale.Fahrenheit:
                        return "°F";
                    case TemperatureScale.Kelvin:
                        return "K";
                    default: return string.Empty;
                }
            }
        }

        public int RefreshInterval
        {
            get { return ClientSettings.RefreshInterval; }
            set { SetProperty(ClientSettings.RefreshInterval, value, () => ClientSettings.RefreshInterval = value); }
        }

        public string Hostname
        {
            get { return ClientSettings.Hostname; }
            set { SetProperty(ClientSettings.Hostname, value, () => ClientSettings.Hostname = value); }
        }

        public int Port
        {
            get { return ClientSettings.Port; }
            set { SetProperty(ClientSettings.Port, value, () => ClientSettings.Port = value); }
        }

        public async void ProbeForHub()
        {
            ProgressViewModel progress = ProgressViewModel.Instance;
            progress.BlockingProgressText = "Searching for hub";
            progress.IsBlockingProgress = true;

            bool didChange = await ClientSettings.ProbeForHub();

            progress.IsBlockingProgress = false;

            // TODO: Pop UI for change
            if (didChange)
            {
                RaisePropertyChanged("Hostname");
                RaisePropertyChanged("Port");
            }
        }
    }
}
