namespace HomeHub.Client
{
    using HomeHub.Shared;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    class DeviceProxy : IDevice
    {
        public string FriendlyName
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public bool IsOnline
        {
            get;
            set;
        }

        public DateTime LastHeartBeat
        {
            get;
            set;
        }

        public IReadOnlyDictionary<DeviceFunction, bool> SupportedCommandFunctions
        {
            get;
            set;
        }

        public IEnumerable<Type> SupportedSensorReadings
        {
            get;
            set;
        }

        public Task<bool> SendCommands(IEnumerable<DeviceCommand> commands)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ISensorReading>> TakeReadings()
        {
            throw new NotImplementedException();
        }
    }
}
