using HomeHub.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HomeHub.Shared;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ThermostatProxy _proxy;
        private AppSettings _settings;

        public ThermostatProxy Proxy
        {
            get
            {
                return _proxy;
            }

            set
            {
                _proxy = value;
                OnPropertyChanged();
            }
        }

        public AppSettings Settings
        {
            get
            {
                return _settings;
            }

            set
            {
                _settings = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            Settings = HomeHub.Client.AppSettings.Instance;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            AppSettings.DataContext = Settings;
            await UpdateProxy();
        }

        private async Task UpdateProxy()
        {
            DataContractJsonSerializer jsonizer = new DataContractJsonSerializer(typeof(ThermostatProxy));
            Uri geturi = new Uri("http://192.168.4.181:8800/api/thermostat");
            HttpClient client = new HttpClient();
            HttpResponseMessage responseGet = await client.GetAsync(geturi);
            var response = await responseGet.Content.ReadAsStringAsync();
            //string response = "{\"PollingTime\":60,\"TargetBufferTime\":120,\"UseRules\":false,\"Devices\":[{\"FriendlyName\":\"Hub\",\"Id\":\"Hub\",\"IsOnline\":true,\"LastHeartBeat\":\"2017-01-31T22:24:47.8598496-08:00\",\"SupportedCommandFunctions\":{\"Heat\":true,\"Fan\":false},\"SupportedSensorReadings\":[\"HomeHub.Shared.TemperatureReading, HomeHub.Shared, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null\",\"HomeHub.Shared.MotionReading, HomeHub.Shared, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null\"]}],\"Rules\":[{\"HighTemperature\":{\"Scale\":1,\"Degrees\":76.0},\"LowTemperature\":{\"Scale\":1,\"Degrees\":70.0},\"IsEnabled\":true,\"StartTime\":\"10675199.02:48:05.4775807\",\"EndTime\":\"10675199.02:48:05.4775807\"}],\"CurrentAverageTemperature\":{\"Scale\":1,\"Degrees\":68.0},\"CurrentTemperatures\":[{\"DeviceId\":\"Hub\",\"ReadingTime\":\"2017-01-31T22:24:47.8598496-08:00\",\"Temperature\":{\"Scale\":1,\"Degrees\":68.0}}]}";
            Proxy = JsonConvert.DeserializeObject<ThermostatProxy>(response, new ClientJsonConverter());

            // Update the UI
            // TODO: Find a more elegant solution
            CurrentAverageTemperatureLabel.Text = Proxy.CurrentAverageTemperature.ToString();
            HubSettings.DataContext = Proxy;
            RuleListView.ItemsSource = Proxy.Rules;
        }

        private void AddRuleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RuleDetailPage));
        }

        private void EditRuleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var rule = Proxy.Rules.First();
            Frame.Navigate(typeof(RuleDetailPage), new TransitionInfo() { Rule = rule, IsOverride = rule.Id == "Override" } );
        }

        private void PollingTime_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Changed");
        }

        private void OverrideButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var rules = Proxy.Rules.Where(r => r.Id == "Override");
            IRule rule = null;

            if (rules.Count() > 0)
            {
                rule = rules.First();
            }

            Frame.Navigate(typeof(RuleDetailPage), new TransitionInfo() { Rule = rule, IsOverride = true });
        }
    }
}
