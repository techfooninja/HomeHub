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
using System.Threading;
using Windows.UI.Core;

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

        private Timer _pollingTimer;

        public MainPage()
        {
            this.InitializeComponent();
            Thermostat = new ThermostatViewModel();
            HubSettings = new HubSettingsViewModel();
            ClientSettings = ClientSettingsViewModel.Instance;

            _pollingTimer = new Timer(TimerCallback, null, 0, ClientSettings.RefreshInterval * 1000);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private async void TimerCallback(object state)
        {
            if (String.IsNullOrEmpty(ClientSettings.Hostname))
            {
                return;
            }

            Proxy = await ThermostatProxy.GetUpdates();

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Update the view model
                Thermostat.Thermostat = Proxy;
                HubSettings.UpdateSettings(Thermostat);
            });
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
