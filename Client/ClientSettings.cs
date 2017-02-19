namespace HomeHub.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Windows.Storage;
    using Windows.Networking.Connectivity;
    using Windows.Web.Http;
    using System.Diagnostics;
    using System.Net;

    public static class ClientSettings
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private static string[] _possibleTempFormats = Enum.GetNames(typeof(TemperatureScale));

        public static string[] PossibleTemperatureFormats
        {
            get
            {
                return _possibleTempFormats;
            }
        }

        public static TemperatureScale TemperatureFormat
        {
            get
            {
                return (TemperatureScale)SettingsHelper.GetProperty((int)TemperatureScale.Fahrenheit);
            }
            set
            {
                SettingsHelper.SetProperty((int)value);
            }
        }

        public static int RefreshInterval
        {
            get
            {
                return SettingsHelper.GetProperty<int>(30);
            }
            set
            {
                SettingsHelper.SetProperty(value);
            }
        }

        public static string Hostname
        {
            get
            {
                return SettingsHelper.GetProperty<string>(null);
            }
            set
            {
                SettingsHelper.SetProperty(value);
            }
        }

        public static int Port
        {
            get
            {
                return SettingsHelper.GetProperty<int>(8800);
            }
            set
            {
                SettingsHelper.SetProperty(value);
            }
        }

        public static async Task<bool> ProbeForHub()
        {
            int maxSubnet = 255;
            int port = Port;
            bool changed = false;
            string ipAddress = NetworkHelpers.GetLocalIp();
            string[] ipParts = ipAddress.Split('.');
            Task<string>[] tasks = new Task<string>[maxSubnet];

            for (int i = 0; i < maxSubnet; i++)
            {
                string currentIp = String.Join(".", ipParts[0], ipParts[1], ipParts[2], i);
                tasks[i] = Task.Run(async () => { Debug.WriteLine("Checking " + currentIp); return await NetworkHelpers.Ping(currentIp); });
            }

            for (int j = 0; j < maxSubnet; j++)
            {
                string hostname = await tasks[j];

                if (!String.IsNullOrEmpty(hostname))
                {
                    Uri uri = new Uri(String.Format("http://{0}:{1}/api/probe", hostname, port));
                    var response = await NetworkHelpers.SendRequest(RequestType.Get, uri, null);

                    if (response != null && response.IsSuccessStatusCode && !hostname.Equals(Hostname, StringComparison.OrdinalIgnoreCase))
                    {
                        Hostname = hostname;
                        changed = true;
                        break;
                    }
                }
            }

            return changed;
        }
    }
}
