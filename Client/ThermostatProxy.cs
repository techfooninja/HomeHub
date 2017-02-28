namespace HomeHub.Client
{
    using HomeHub.Shared;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
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

        public static async Task<ThermostatProxy> GetUpdates()
        {
            Uri uri = new Uri(String.Format("http://{0}:{1}/api/thermostat", ClientSettings.Hostname, ClientSettings.Port));
            var response = await NetworkHelpers.SendRequest(RequestType.Get, uri, null);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ThermostatProxy>(responseString, new ClientJsonConverter());
        }

        public static async Task<HttpResponseMessage> AddRule(Rule rule)
        {
            Uri uri = new Uri(String.Format("http://{0}:{1}/api/thermostat/addrule", ClientSettings.Hostname, ClientSettings.Port));
            var jsonContent = JsonConvert.SerializeObject(rule);
            var response = await NetworkHelpers.SendRequest(RequestType.Put, uri, jsonContent);
            return response;
        }

        public static async Task<HttpResponseMessage> UpdateRule(Rule rule)
        {
            Uri uri = new Uri(String.Format("http://{0}:{1}/api/thermostat/updaterule", ClientSettings.Hostname, ClientSettings.Port));
            var jsonContent = JsonConvert.SerializeObject(rule);
            var response = await NetworkHelpers.SendRequest(RequestType.Post, uri, jsonContent);
            return response;
        }

        public static async Task<HttpResponseMessage> DeleteRule(Rule rule)
        {
            Uri uri = new Uri(String.Format("http://{0}:{1}/api/thermostat/deleterule/{2}", ClientSettings.Hostname, ClientSettings.Port, rule.Id));
            var response = await NetworkHelpers.SendRequest(RequestType.Delete, uri, null);
            return response;
        }

        public static async Task<HttpResponseMessage> SetHoldTemp(Rule rule, DateTime expirationDate)
        {
            Uri uri = new Uri(String.Format("http://{0}:{1}/api/thermostat/setholdtemp?expiration={2}", ClientSettings.Hostname, ClientSettings.Port, expirationDate.ToString("o")));
            var jsonContent = JsonConvert.SerializeObject(rule);
            var response = await NetworkHelpers.SendRequest(RequestType.Put, uri, jsonContent);
            return response;
        }
    }
}
