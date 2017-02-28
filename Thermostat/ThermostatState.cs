namespace HomeHub.Hub
{
    using Shared;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    class ThermostatState
    {
        [DataMember]
        public List<Rule> Rules;

        [DataMember]
        public int PollingTime;

        [DataMember]
        public int TargetBufferTime;

        [DataMember]
        public bool UseRules;
    }
}
