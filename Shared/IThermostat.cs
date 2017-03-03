namespace HomeHub.Shared
{
    using System.Collections.Generic;

    public interface IThermostat
    {
        int PollingTime { get; }
        int TargetBufferTime { get; }
        bool UseRules { get; }
        IEnumerable<Device> Devices { get; }
        IEnumerable<Rule> Rules { get; }
        string CurrentRuleId { get; }
        Temperature CurrentAverageTemperature { get; }
        IEnumerable<TemperatureReading> CurrentTemperatures { get; }
    }
}
