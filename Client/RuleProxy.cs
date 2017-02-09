namespace HomeHub.Client
{
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Controls;

    class RuleProxy : IRule
    {
        public RuleProxy()
        {
            StartTime = DateTime.Now.TimeOfDay;
            EndTime = StartTime.Add(new TimeSpan(1, 0, 0));
            LowTemperature = new Temperature(AppSettings.Instance.TemperatureFormat, 70);
            HighTemperature = new Temperature(AppSettings.Instance.TemperatureFormat, 76);
            IsEnabled = true;
        }

        public TimeSpan EndTime
        {
            get;
            set;
        }

        public Temperature HighTemperature
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        public Temperature LowTemperature
        {
            get;
            set;
        }

        public TimeSpan StartTime
        {
            get;
            set;
        }

        public Symbol RuleSymbol
        {
            get
            {
                switch (Id)
                {
                    case "Default": return Symbol.Pin;
                    case "Override": return Symbol.ReportHacked;
                    default: return Symbol.Clock;
                }
            }
        }

        public Temperature LowTemperatureFormatted
        {
            get
            {
                return LowTemperature.ConvertToScale(AppSettings.Instance.TemperatureFormat);
            }
            set
            {
                LowTemperature = value;
            }
        }

        public Temperature HighTemperatureFormatted
        {
            get
            {
                return HighTemperature.ConvertToScale(AppSettings.Instance.TemperatureFormat);
            }
            set
            {
                HighTemperature = value;
            }
        }

        public bool IsApplicableNow()
        {
            throw new NotImplementedException();
        }

        public TemperatureState ProcessReadings(IEnumerable<ISensorReading> readings)
        {
            throw new NotImplementedException();
        }
    }
}
