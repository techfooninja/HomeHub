﻿namespace HomeHub.Client
{
    using System;
    using HomeHub.Shared;
    using System.Threading.Tasks;

    public class HubSettings
    {
        private int _pollingTime;
        private int _targetBufferTime;
        private bool _useRules;

        public int PollingTime
        {
            get
            {
                return _pollingTime;
            }
            set
            {
                if (_pollingTime != value)
                {
                    _pollingTime = value;
                    Uri uri = new Uri(String.Format("http://{0}:{1}/api/thermostat/setpollingtime", ClientSettings.Hostname, ClientSettings.Port));
                    NetworkHelpers.SendRequest(RequestType.Put, uri, value.ToString());
                }
            }
        }

        public int TargetBufferTime
        {
            get
            {
                return _targetBufferTime;
            }
            set
            {
                if (_targetBufferTime != value)
                {
                    _targetBufferTime = value;
                    Uri uri = new Uri(String.Format("http://{0}:{1}/api/thermostat/settargetbuffertime", ClientSettings.Hostname, ClientSettings.Port));
                    NetworkHelpers.SendRequest(RequestType.Put, uri, value.ToString());
                }
            }
        }

        public bool UseRules
        {
            get
            {
                return _useRules;
            }
            set
            {
                if (_useRules != value)
                {
                    _useRules = value;
                    Uri uri = new Uri(String.Format("http://{0}:{1}/api/thermostat/setuserules", ClientSettings.Hostname, ClientSettings.Port));
                    NetworkHelpers.SendRequest(RequestType.Put, uri, (value ? "true" : "false"));
                }
            }
        }

        public void UpdateSettings(ThermostatProxy proxy)
        {
            _pollingTime = proxy.PollingTime;
            _targetBufferTime = proxy.TargetBufferTime;
            _useRules = proxy.UseRules;
        }
    }
}
