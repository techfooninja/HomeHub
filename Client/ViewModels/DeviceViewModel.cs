namespace HomeHub.Client.ViewModels
{
    using HomeHub.Shared;
    using System;
    using System.Collections.Generic;

    public class DeviceViewModel : NotificationBase<Device>
    {
        public DeviceViewModel(Device device = null) : base(device)
        {
            Device = device;
        }

        public Device Device
        {
            get { return This; }
            set
            {
                SetProperty(This, value, () => This = value);
                RaisePropertyChanged("FriendlyName");
                RaisePropertyChanged("IsOnline");
                RaisePropertyChanged("LastHeartBeat");
                RaisePropertyChanged("UsageMinutes");
                RaisePropertyChanged("MaxUsageMinutes");
                RaisePropertyChanged("SensorTypes");
                RaisePropertyChanged("FunctionStatus");
            }
        }

        public string FriendlyName
        {
            get { return This.FriendlyName; }
            set { SetProperty(This.FriendlyName, value, () => This.FriendlyName = value); }
        }

        public bool IsOnline
        {
            get { return This.IsOnline; }
        }

        public DateTime LastHeartBeat
        {
            get { return This.LastHeartBeat; }
        }

        public int UsageMinutes
        {
            get { return This.UsageMinutes; }
        }

        public int MaxUsageMinutes
        {
            get { return This.UsageMinutes; }
            set { SetProperty(This.MaxUsageMinutes, value, () => This.MaxUsageMinutes = value); }
        }

        public IEnumerable<Type> SensorTypes
        {
            get { return This.SupportedSensorReadings; }
        }

        public IReadOnlyDictionary<DeviceFunction,bool> FunctionStatus
        {
            get { return This.SupportedCommandFunctions; }
        }
    }
}
