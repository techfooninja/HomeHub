namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Storage;

    public static class SettingsHelper
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static object GetProperty(object defaultValue, [CallerMemberName] String propertyName = null)
        {
            if (!localSettings.Values.ContainsKey(propertyName))
            {
                localSettings.Values[propertyName] = defaultValue;
            }
            return localSettings.Values[propertyName];
        }

        public static T GetProperty<T>(T defaultValue, [CallerMemberName] String propertyName = null)
        {
            return (T)GetProperty(defaultValue, propertyName);
        }

        public static void SetProperty(object value, [CallerMemberName] String propertyName = null)
        {
            if (!String.IsNullOrEmpty(propertyName))
            {
                localSettings.Values[propertyName] = value;
            }
        }
    }
}
