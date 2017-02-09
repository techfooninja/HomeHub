namespace HomeHub.Hub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;

    [DataContract]
    class DefaultRule : IRule
    {
        // MaxValue will guarantee that this rule is at the end of the list
        private static TimeSpan _maxTime = TimeSpan.MaxValue;
        private string _id;

        public DefaultRule()
        {
            LowTemperature = new Temperature(TemperatureScale.Fahrenheit, 70);
            HighTemperature = new Temperature(TemperatureScale.Fahrenheit, 76);

            Id = "Default";
        }

        [DataMember]
        public Temperature HighTemperature
        {
            get;
            set;
        }

        [DataMember]
        public Temperature LowTemperature
        {
            get;
            set;
        }

        [DataMember]
        public bool IsEnabled
        {
            get { return true; }
            set { /* Do nothing */ }
        }

        [DataMember]
        public TimeSpan StartTime
        {
            get { return _maxTime; }
            set { /* Do Nothing */ }
        }

        [DataMember]
        public TimeSpan EndTime
        {
            get { return _maxTime; }
            set { /* Do Nothing */ }
        }

        [DataMember]
        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                if (value != null)
                {
                    _id = value;
                }
            }
        }

        public bool IsApplicableNow()
        {
            return true;
        }
        
        public TemperatureState ProcessReadings(IEnumerable<ISensorReading> readings)
        {
            Temperature average =
                Temperature.Average(
                    readings.OfType<TemperatureReading>()
                    .Select((tr) => tr.Temperature)
                    .AsEnumerable()
                );
            
            if (average < LowTemperature)
            {
                return TemperatureState.Low;
            }
            
            if (average > HighTemperature)
            {
                return TemperatureState.High;
            }

            return TemperatureState.Target;
        }
    }
}
