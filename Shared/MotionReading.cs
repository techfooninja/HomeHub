namespace HomeHub.Shared
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class MotionReading : ISensorReading
    {
        public MotionReading(string deviceId, bool hasMotion)
        {
            ReadingTime = DateTime.Now;
            DeviceId = deviceId;
            HasMotion = hasMotion;
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
        public bool HasMotion
        {
            get;
            private set;
        }
    }
}
