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
                return SettingsHelper.GetProperty<TemperatureScale>(TemperatureScale.Fahrenheit);
            }
            set
            {
                SettingsHelper.SetProperty(value);
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
            Task<bool>[] tasks = new Task<bool>[maxSubnet];

            for (int i = 0; i < maxSubnet; i++)
            {
                string currentIp = String.Join(".", ipParts[0], ipParts[1], ipParts[2], i);
                tasks[i] = Task.Run(async () => { Debug.WriteLine("Checking " + currentIp); return await NetworkHelpers.Ping(currentIp); });
            }

            for (int j = 0; j < maxSubnet; j++)
            {
                Debug.WriteLine("Before Await");
                bool result = await tasks[j];
                Debug.WriteLine("After Await");
                // TODO: Break this up into two methods? Maybe there's something about spending too much time in a single method?

                if (result)
                {
                    string currentIp = String.Join(".", ipParts[0], ipParts[1], ipParts[2], j);
                    Debug.WriteLine("Probing " + currentIp);
                    var host = await Dns.GetHostEntryAsync(currentIp);
                    Uri uri = new Uri(String.Format("http://{0}:{1}/api/probe", host.HostName, port));
                    var response = NetworkHelpers.SendRequest(RequestType.Get, uri, null);

                    if (response != null && host.HostName != Hostname)
                    {
                        Hostname = host.HostName;
                        changed = true;
                        break;
                    }
                }
            }

            return changed;
        }
    }
}
