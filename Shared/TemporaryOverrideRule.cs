namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [DataContract]
    public class TemporaryOverrideRule : Rule
    {
        private static TimeSpan _minTime = TimeSpan.MinValue;

        public TemporaryOverrideRule()
        {
            Id = "Override";
        }
        
        [DataMember]
        public DateTime Expiration
        {
            get;
            set;
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
