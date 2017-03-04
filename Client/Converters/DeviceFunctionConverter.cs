namespace HomeHub.Client.Converters
{
    using Shared;
    using System;
    using System.Collections.Generic;
    using Windows.UI.Xaml.Data;

    public class DeviceFunctionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            KeyValuePair<DeviceFunction, bool> kvp = (KeyValuePair<DeviceFunction, bool>)value;
            return String.Format("{0}: {1}", Enum.GetName(typeof(DeviceFunction), kvp.Key), kvp.Value ? "On" : "Off");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
