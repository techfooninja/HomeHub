namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRule
    {
        string Id { get; }
        Temperature LowTemperature { get; set; }
        Temperature HighTemperature { get; set; }
        bool IsEnabled { get; set; }
        TimeSpan StartTime { get; set; }
        TimeSpan EndTime { get; set; }

        bool IsApplicableNow();
        TemperatureState ProcessReadings(IEnumerable<ISensorReading> readings);
    }
}
