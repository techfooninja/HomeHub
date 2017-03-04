namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class Rule
    {
        protected static TimeSpan _oneDay = new TimeSpan(1, 0, 0, 0);

        protected TimeSpan _startTime;
        protected TimeSpan _endTime;
        protected string _id;

        public Rule()
        {

        }

        public Rule(Rule newRule)
        {
            CopyFromRule(newRule);
        }

        public virtual void CopyFromRule(Rule newRule)
        {
            Id = newRule.Id;
            StartTime = newRule.StartTime;
            EndTime = newRule.EndTime;
            HighTemperature = newRule.HighTemperature;
            LowTemperature = newRule.LowTemperature;
            IsEnabled = newRule.IsEnabled;
            Expiration = newRule.Expiration;
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
        public Temperature LowTemperature { get; set; }

        [DataMember]
        public Temperature HighTemperature { get; set; }

        [DataMember]
        public virtual bool IsEnabled { get; set; }

        [DataMember]
        public virtual TimeSpan StartTime
        {
            get
            {
                return _startTime;
            }

            set
            {
                if (value >= _oneDay)
                {
                    _startTime = value - _oneDay;
                }
                else
                {
                    _startTime = value;
                }
            }
        }

        [DataMember]
        public virtual TimeSpan EndTime
        {
            get
            {
                return _endTime;
            }

            set
            {
                if (value >= _oneDay)
                {
                    _endTime = value - _oneDay;
                }
                else
                {
                    _endTime = value;
                }
            }
        }

        [DataMember]
        public virtual DateTime Expiration
        {
            get;
            set;
        }

        public static Type GetTypeById(string id)
        {
            if (id == "Default")
                return typeof(DefaultRule);
            else if (id == "Override")
                return typeof(TemporaryOverrideRule);
            else if (id != null && id.StartsWith("Schedule"))
                return typeof(ScheduleRule);
            else
                return typeof(Rule);
        }

        public virtual bool IsApplicableNow()
        {
            TimeSpan now = DateTime.Now.TimeOfDay;

            if (StartTime < EndTime)
            {
                // Doesn't cross midnight threshold
                if (now >= StartTime && now < EndTime)
                {
                    return true;
                }

                return false;
            }
            else
            {
                // Does cross midnight threshold
                if (now < StartTime || now >= EndTime)
                {
                    return true;
                }

                return false;
            }
        }

        public virtual TemperatureState ProcessReadings(IEnumerable<ISensorReading> readings)
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
