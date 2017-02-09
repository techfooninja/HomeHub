namespace HomeHub.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class AppSettings : INotifyPropertyChanged
    {
        private AppSettings()
        {

        }

        private static AppSettings _instance = null;

        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppSettings();
                }
                return _instance;
            }
        }

        private static string[] _possibleTempFormats = Enum.GetNames(typeof(TemperatureScale));

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] PossibleTemperatureFormats
        {
            get
            {
                return _possibleTempFormats;
            }
        }

        public TemperatureScale TemperatureFormat
        {
            get;
            set;
        }

        public int TemperatureFormatIndex
        {
            get
            {
                return (int)TemperatureFormat;
            }
            set
            {
                TemperatureFormat = (TemperatureScale)value;
                OnPropertyChanged();
                OnPropertyChanged("TemperatureFormat");
                OnPropertyChanged("TemperatureFormatUnit");
            }
        }

        public string TemperatureFormatUnit
        {
            get
            { 
                switch (TemperatureFormat)
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

        public int RefreshInterval { get; set; }
    }
}
