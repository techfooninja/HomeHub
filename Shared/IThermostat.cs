namespace HomeHub.Shared
{
    using System.Collections.Generic;

    public interface IThermostat
    {
        int PollingTime { get; }
        int TargetBufferTime { get; }
        bool UseRules { get; }
        IEnumerable<IDevice> Devices { get; }
        IEnumerable<Rule> Rules { get; }
        Temperature CurrentAverageTemperature { get; }
        IEnumerable<TemperatureReading> CurrentTemperatures { get; }
    }
}
