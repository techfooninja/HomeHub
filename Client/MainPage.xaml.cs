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
using HomeHub.Client.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ThermostatProxy Proxy
        {
            get;
            set;
        }

        public ThermostatViewModel Thermostat
        {
            get;
            set;
        }

        public HubSettingsViewModel HubSettings
        {
            get;
            set;
        }

        public ClientSettingsViewModel ClientSettings
        {
            get;
            set;
        }

        public MainPage()
        {
            this.InitializeComponent();
            Thermostat = new ThermostatViewModel();
            HubSettings = new HubSettingsViewModel();
            ClientSettings = ClientSettingsViewModel.Instance;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await UpdateProxy();
        }

        private async Task UpdateProxy()
        {

            Proxy = await ThermostatProxy.GetUpdates();

            // Update the view model
            HubSettings.PollingTime = Proxy.PollingTime;
            HubSettings.TargetBufferTime = Proxy.TargetBufferTime;
            HubSettings.UseRules = Proxy.UseRules;
            Thermostat.CurrentTemperature = Proxy.CurrentAverageTemperature;
            Thermostat.ReloadRules(Proxy.Rules);
        }

        private void AddRuleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RuleDetailPage));
        }

        private void EditRuleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var rule = (RuleViewModel)sender;
            Frame.Navigate(typeof(RuleDetailPage), new TransitionInfo() { Rule = rule, IsOverride = rule.RuleType == typeof(TemporaryOverrideRule) } );
        }

        private void PollingTime_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Changed");
        }

        private void OverrideButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var rules = Thermostat.Rules.Where(r => r.RuleType == typeof(TemporaryOverrideRule));
            RuleViewModel rule = null;

            if (rules.Count() > 0)
            {
                rule = rules.First();
            }

            Frame.Navigate(typeof(RuleDetailPage), new TransitionInfo() { Rule = rule, IsOverride = true });
        }
    }
}
