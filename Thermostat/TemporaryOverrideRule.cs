namespace HomeHub.Hub
{
    using HomeHub.Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract]
    class TemporaryOverrideRule : IRule
    {
        private static TimeSpan _minTime = TimeSpan.MinValue;
        private string _id;

        public TemporaryOverrideRule()
        {
            Id = "Override";
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

        [DataMember]
        public DateTime Expiration
        {
            get;
            set;
        }

        [DataMember]
        public TimeSpan EndTime
        {
            get
            {
                return _minTime;
            }

            set
            {
                /* Do nothing */
            }
        }

        [DataMember]
        public Temperature HighTemperature
        {
            get;
            set;
        }

        [DataMember]
        public bool IsEnabled
        {
            get
            {
                return true;
            }

            set
            {
                /* Do nothing */
            }
        }

        [DataMember]
        public Temperature LowTemperature
        {
            get;
            set;
        }

        [DataMember]
        public TimeSpan StartTime
        {
            get
            {
                return _minTime;
            }

            set
            {
                /* Do nothing */
            }
        }

        public bool IsApplicableNow()
        {
            return DateTime.Now < Expiration;
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
