namespace HomeHub.Shared
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class TemporaryOverrideRule : Rule
    {
        private static TimeSpan _minTime = TimeSpan.Zero;

        public TemporaryOverrideRule()
        {
            Id = "Override";
        }

        [DataMember]
        public override TimeSpan StartTime
        {
            get
            {
                return _minTime;
            }
        }

        [DataMember]
        public override TimeSpan EndTime
        {
            get
            {
                return _minTime;
            }
        }

        [DataMember]
        public override bool IsEnabled
        {
            get
            {
                return true;
            }
        }

        public override bool IsApplicableNow()
        {
            return DateTime.Now < Expiration;
        }
    }
}
