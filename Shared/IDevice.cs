namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Windows.Foundation;

    public interface IDevice
    {
        string Id
        {
            get;
        }

        string FriendlyName
        {
            get;
            set;
        }

        bool IsOnline
        {
            get;
        }

        DateTime LastHeartBeat
        {
            get;
        }

        IEnumerable<Type> SupportedSensorReadings
        {
            get;
        }

        Task<IEnumerable<ISensorReading>> TakeReadings();

        IReadOnlyDictionary<DeviceFunction,bool> SupportedCommandFunctions
        {
            get;
        }

        Task<bool> SendCommands(IEnumerable<DeviceCommand> commands);
    }
}
