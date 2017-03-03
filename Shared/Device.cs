namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [DataContract]
    public class Device
    {
        protected Device()
        {
            SupportedSensorReadings = new List<Type>();
            SupportedCommandFunctions = new Dictionary<DeviceFunction, bool>();
        }

        [DataMember]
        public string Id
        {
            get;
            protected set;
        }

        [DataMember]
        public string FriendlyName
        {
            get;
            set;
        }

        [DataMember]
        public bool IsOnline
        {
            get;
            protected set;
        }

        [DataMember]
        public DateTime LastHeartBeat
        {
            get;
            protected set;
        }

        [DataMember]
        public IEnumerable<Type> SupportedSensorReadings
        {
            get;
            protected set;
        }

        public virtual Task<IEnumerable<ISensorReading>> TakeReadings()
        {
            throw new NotImplementedException("Cannot call TakeReadings on a base Device object");
        }

        public IReadOnlyDictionary<DeviceFunction,bool> SupportedCommandFunctions
        {
            get;
            protected set;
        }

        public virtual Task<bool> SendCommands(IEnumerable<DeviceCommand> commands)
        {
            throw new NotImplementedException("Cannot call SendCommands on a base Device object");
        }
    }
}
