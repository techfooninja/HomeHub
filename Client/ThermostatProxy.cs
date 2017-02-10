namespace HomeHub.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Windows.Web.Http;
    
    [DataContract]
    public class ThermostatProxy : IThermostat
    {
        [DataMember]
        public Temperature CurrentAverageTemperature
        {
            get;
            protected set;
        }

        [DataMember]
        public IEnumerable<TemperatureReading> CurrentTemperatures
        {
            get;
            protected set;
        }

        [DataMember]
        public IEnumerable<IDevice> Devices
        {
            get;
            protected set;
        }

        [DataMember]
        public int PollingTime
        {
            get;
            set;
        }

        [DataMember]
        public IEnumerable<Rule> Rules
        {
            get;
            protected set;
        }

        [DataMember]
        public int TargetBufferTime
        {
            get;
            set;
        }

        [DataMember]
        public bool UseRules
        {
            get;
            set;
        }

        private enum RequestType
        {
            Get,
            Post,
            Put,
            Delete
        }

        public static async Task<ThermostatProxy> GetUpdates()
        {
            Uri uri = new Uri("http://192.168.4.181:8800/api/thermostat");
            var response = await SendRequest(RequestType.Get, uri, null);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ThermostatProxy>(responseString, new ClientJsonConverter());
        }

        public static async Task<HttpResponseMessage> AddRule(Rule rule)
        {
            Uri uri = new Uri("http://192.168.4.181:8800/api/thermostat/addrule");
            var jsonContent = JsonConvert.SerializeObject(rule);
            var response = await SendRequest(RequestType.Put, uri, jsonContent);
            return response;
        }

        public static async Task<HttpResponseMessage> UpdateRule(Rule rule)
        {
            Uri uri = new Uri("http://192.168.4.181:8800/api/thermostat/updaterule");
            var jsonContent = JsonConvert.SerializeObject(rule);
            var response = await SendRequest(RequestType.Post, uri, jsonContent);
            return response;
        }

        public static async Task<HttpResponseMessage> SetHoldTemp(Rule rule, DateTime expirationDate)
        {
            Uri uri = new Uri(String.Format("http://192.168.4.181:8800/api/thermostat/setholdtemp?expiration={0}", expirationDate.ToString("o")));
            var jsonContent = JsonConvert.SerializeObject(rule);
            var response = await SendRequest(RequestType.Put, uri, jsonContent);
            return response;
        }

        private static async Task<HttpResponseMessage> SendRequest(RequestType type, Uri uri, string content)
        {
            HttpClient client = new HttpClient();
            using (var httpContent = new HttpStringContent(content, Windows.Storage.Streams.UnicodeEncoding.Utf8))
            {
                HttpResponseMessage response = null;

                switch (type)
                {
                    case RequestType.Get: response = await client.GetAsync(uri); break;
                    case RequestType.Post: response = await client.PostAsync(uri, httpContent); break;
                    case RequestType.Put: response = await client.PutAsync(uri, httpContent); break;
                    case RequestType.Delete: response = await client.DeleteAsync(uri); break;
                    default: break;
                }
                
                return response;
            }
        }
    }
}
