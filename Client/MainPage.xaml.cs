// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Client
{
    using HomeHub.Client;
    using HomeHub.Client.ViewModels;
    using HomeHub.Shared;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Windows.System.Profile;
    using Windows.UI.Core;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool _didPromptForScan = false;

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

            // Tie polling timer to client settings change
            ClientSettings.PropertyChanged += ClientSettings_PropertyChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TimerCallback(null);
        }

        private void ClientSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RefreshInterval")
            {
                _pollingTimer.Change(0, ClientSettings.RefreshInterval * 1000);
            }
        }

        private async void TimerCallback(object state)
        {
            if (String.IsNullOrEmpty(ClientSettings.Hostname))
            {
                if (!_didPromptForScan)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        _didPromptForScan = true;
                        var msg = new MessageDialog("Would you like to search for a hub or do you know your hub's hostname?", "Welcome!");
                        msg.Commands.Add(new UICommand() { Id = "search", Label = "Search for hub" });
                        msg.Commands.Add(new UICommand() { Id = "hostname", Label = "I know the hostname" });
                        msg.DefaultCommandIndex = 0;

                        // TODO: Revisit if MessageDialog API is updated in future release
                        var deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
                        if (deviceFamily.Contains("Desktop"))
                        {
                            msg.Commands.Add(new UICommand() { Id = "cancel", Label = "Cancel" });
                            msg.CancelCommandIndex = 2;
                        }
                        // Maybe Xbox 'B' button works, but I don't know so best to not do anything
                        else if (!deviceFamily.Contains("Mobile"))
                        {
                            throw new Exception("Don't know how to show dialog for device "
                              + deviceFamily);
                        }

                        var result = await msg.ShowAsync();

                        if (result == null || (string)result.Id == "cancel")
                        {
                            Debug.WriteLine("Cancel Pressed");
                        }
                        else if ((string)result.Id == "search")
                        {
                            Debug.WriteLine("Search Pressed");
                            MainPivot.SelectedItem = SettingsPivot;
                            ClientSettings.ProbeForHub();
                        }
                        else if ((string)result.Id == "hostname")
                        {
                            Debug.WriteLine("Hostname Pressed");
                            MainPivot.SelectedItem = SettingsPivot;
                        }
                    });
                }

                return;
            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                Progress.NonBlockingProgressText = "Getting updates from hub...";
                Progress.IsNonBlockingProgress = true;

                await Thermostat.GetUpdates();

                // Update the view model
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
