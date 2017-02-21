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

        public ProgressViewModel Progress
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
            Progress = ProgressViewModel.Instance;

            _pollingTimer = new Timer(TimerCallback, null, 0, ClientSettings.RefreshInterval * 1000);
        }

        private async void TimerCallback(object state)
        {
            if (String.IsNullOrEmpty(ClientSettings.Hostname))
            {
                return;
            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Progress.NonBlockingProgressText = "Getting updates from hub...";
                Progress.IsNonBlockingProgress = true;
            });

            Proxy = await ThermostatProxy.GetUpdates();

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Update the view model
                Thermostat.Thermostat = Proxy;
                HubSettings.UpdateSettings(Thermostat);
                Progress.IsNonBlockingProgress = false;
            });
        }

        private void AddRuleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavigateToRuleDetail();
        }

        private void EditRuleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var rule = (RuleViewModel)sender;
            NavigateToRuleDetail(new TransitionInfo() { Rule = rule, IsOverride = rule.RuleType == typeof(TemporaryOverrideRule) });
        }

        private void NavigateToRuleDetail(TransitionInfo info = null)
        {
            Frame.Navigate(typeof(RuleDetailPage), info);
        }

        private void PollingTime_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Changed");
        }

        private void OverrideButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RuleViewModel rule = null;
            if (Thermostat.Rules[0].RuleType == typeof(TemporaryOverrideRule))
            {
                rule = Thermostat.Rules[0];
            }
            Frame.Navigate(typeof(RuleDetailPage), new TransitionInfo() { Rule = rule, IsOverride = true });
        }

        private async void StartBlockingProgress_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Progress.BlockingProgressText = "Reloading data";
            Progress.IsBlockingProgress = true;

            await Task.Delay(10000);

            Progress.IsBlockingProgress = false;
        }

        private async void StartNonBlockingProgress_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Progress.NonBlockingProgressText = "Refreshing data";
            Progress.IsNonBlockingProgress = true;

            await Task.Delay(10000);

            Progress.IsNonBlockingProgress = false;
        }

        private void RuleListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListView list = (ListView)sender;
            RuleViewModel rule = (RuleViewModel)e.ClickedItem;
            NavigateToRuleDetail(new TransitionInfo() { Rule = rule, IsOverride = rule.IsOverride });
        }
    }
}
