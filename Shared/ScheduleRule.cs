namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract]
    public class ScheduleRule : Rule
    {
        public ScheduleRule()
        {
            IsEnabled = false;
            // TODO: Add ApplicableDevices logic
            // ApplicableDevices = Thermostat.Instance.Devices;
            Id = String.Format("Schedule_{0}", IdCounter);
        }

        private static int _idCounter = 1;

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

        public override DateTime Expiration
        {
            get
            {
                return default(DateTime);
            }
            set
            {
                /* Do Nothing */
            }
        }

        [DataMember]
        public IEnumerable<IDevice> ApplicableDevices
        {
            get;
            set;
        }

        public override bool IsApplicableNow()
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

        public override TemperatureState ProcessReadings(IEnumerable<ISensorReading> readings)
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
