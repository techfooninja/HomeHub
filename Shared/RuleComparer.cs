namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;

    public class RuleComparer : Comparer<IRule>
    {
        public override int Compare(IRule x, IRule y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            var startTimeCompare = Comparer<TimeSpan>.Default.Compare(x.StartTime, y.StartTime);
            var endTimeCompare = Comparer<TimeSpan>.Default.Compare(x.EndTime, y.EndTime);

            if (startTimeCompare == 0)
            {
                return endTimeCompare;
            }

            return startTimeCompare;
        }
    }
}
