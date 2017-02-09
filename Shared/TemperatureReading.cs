namespace HomeHub.Shared
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class TemperatureReading : ISensorReading
    {
        public TemperatureReading(string deviceId, Temperature temp)
        {
            ReadingTime = DateTime.Now;
            DeviceId = deviceId;
            Temperature = temp;
        }

        [DataMember]
        public string DeviceId
        {
            get;
            private set;
        }

        [DataMember]
        public DateTime ReadingTime
        {
            get;
            private set;
        }

        [DataMember]
        public Temperature Temperature
        {
            get;
            private set;
        }
    }
}
