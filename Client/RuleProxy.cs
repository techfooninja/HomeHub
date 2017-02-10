namespace HomeHub.Client
{
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Controls;

    class RuleProxy : Rule
    {
        public RuleProxy()
        {
            StartTime = DateTime.Now.TimeOfDay;
            EndTime = StartTime.Add(new TimeSpan(1, 0, 0));
            //LowTemperature = new Temperature(ClientSettings.Instance.TemperatureFormat, 70);
            //HighTemperature = new Temperature(ClientSettings.Instance.TemperatureFormat, 76);
            IsEnabled = true;
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
