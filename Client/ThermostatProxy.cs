namespace HomeHub.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;
    using System.Runtime.Serialization;

    [DataContract]
    public class ThermostatProxy : IThermostat
    {
        [DataMember]
        public Temperature CurrentAverageTemperature
        {
            get;
            protected set;
        }

        [DataMember]
        public IEnumerable<TemperatureReading> CurrentTemperatures
        {
            get;
            protected set;
        }

        [DataMember]
        public IEnumerable<IDevice> Devices
        {
            get;
            protected set;
        }

        [DataMember]
        public int PollingTime
        {
            get;
            set;
        }

        [DataMember]
        public IEnumerable<IRule> Rules
        {
            get;
            protected set;
        }

        [DataMember]
        public int TargetBufferTime
        {
            get;
            set;
        }

        [DataMember]
        public bool UseRules
        {
            get;
            set;
        }
    }
}
