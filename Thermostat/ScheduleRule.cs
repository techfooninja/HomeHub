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
    class ScheduleRule : IRule
    {
        public ScheduleRule()
        {
            IsEnabled = false;
            ApplicableDevices = Thermostat.Instance.Devices;
            Id = String.Format("Schedule_{0}", IdCounter);
        }

        private static int _idCounter = 1;
        private static TimeSpan _oneDay = new TimeSpan(1, 0, 0, 0);

        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private string _id;
        
        private static int IdCounter
        {
            get
            {
                return _idCounter++;
            }
            set
            {
                _idCounter = value;
            }
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
            get;
            set;
        }

        [DataMember]
        public TimeSpan StartTime
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
        public TimeSpan EndTime
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
        public IEnumerable<IDevice> ApplicableDevices
        {
            get;
            set;
        }

        [DataMember]
        public string Id
        {
            get
            {
                return _id;
            }

            private set
            {
                if (value != null)
                {
                    _id = value;
                }
            }
        }

        public bool IsApplicableNow()
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

        public TemperatureState ProcessReadings(IEnumerable<ISensorReading> readings)
        {
            Temperature average =
                Temperature.Average(
                    readings.OfType<TemperatureReading>()
                    .Where((tr) => ApplicableDevices.Where(d => d.Id == tr.DeviceId).Count() > 0)
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
