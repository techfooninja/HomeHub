namespace HomeHub.Client.Converters
{
    using HomeHub.Shared;
    using System;
    using Windows.UI.Xaml.Data;

    public class SensorTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Type sensorType = (Type)value;

            if (sensorType == typeof(TemperatureReading))
            {
                return "Thermometer";
            }
            else if (sensorType == typeof(MotionReading))
            {
                return "Motion Sensor";
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
