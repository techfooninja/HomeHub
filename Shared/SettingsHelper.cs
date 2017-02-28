namespace HomeHub.Shared
{
    using Newtonsoft.Json;
    using System;
    using System.Runtime.CompilerServices;
    using Windows.Storage;

    public static class SettingsHelper
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public static T GetProperty<T>(T defaultValue, bool deserialize = false, [CallerMemberName] String propertyName = null)
        {
            if (!localSettings.Values.ContainsKey(propertyName))
            {
                SetProperty(defaultValue, deserialize, propertyName);
            }

            if (deserialize)
            {
                return (T)JsonConvert.DeserializeObject((string)localSettings.Values[propertyName], jsonSettings);
            }
            else
            {
                return (T)localSettings.Values[propertyName];
            }
        }

        public static void SetProperty(object value, bool serialize = false, [CallerMemberName] String propertyName = null)
        {
            if (!String.IsNullOrEmpty(propertyName))
            {
                if (serialize)
                {
                    localSettings.Values[propertyName] = JsonConvert.SerializeObject(value, jsonSettings);
                }
                else
                {
                    localSettings.Values[propertyName] = value;
                }
            }
        }
    }
}
