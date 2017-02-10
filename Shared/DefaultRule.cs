namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract]
    public class DefaultRule : Rule
    {
        // MaxValue will guarantee that this rule is at the end of the list
        private static TimeSpan _maxTime = TimeSpan.MaxValue;

        public DefaultRule()
        {
            LowTemperature = new Temperature(TemperatureScale.Fahrenheit, 70);
            HighTemperature = new Temperature(TemperatureScale.Fahrenheit, 76);

            Id = "Default";
        }

        public override TimeSpan StartTime
        {
            get
            {
                return _maxTime;
            }
        }

        public override TimeSpan EndTime
        {
            get
            {
                return _maxTime;
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        public override bool IsApplicableNow()
        {
            return true;
        }
    }
}
