// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeHub.Client
{
    using HomeHub.Client.ViewModels;
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RuleDetailPage : Page
    {
        public RuleDetailPage()
        {
            this.InitializeComponent();
            ClientSettings = ClientSettingsViewModel.Instance;
            Progress = ProgressViewModel.Instance;
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

        public RuleViewModel Rule
        {
            get;
            set;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                Rule = new RuleViewModel();
                Rule.IsNewRule = true;
            }
            else
            {
                TransitionInfo info = (TransitionInfo)e.Parameter;
                Rule = info.Rule ?? new RuleViewModel();
                Rule.IsNewRule = false;
                Rule.IsOverride = info.IsOverride;
                Rule.IsNewRule = info.Rule == null;
            }

            if (Rule.IsOverride && Rule.ExpirationDateTime == default(DateTime))
            {
                // Set the default expiration date if it isn't already set
                var tempExpiration = DateTime.Now.Add(new TimeSpan(1, 0, 0));
                Rule.ExpirationDate = tempExpiration.Date;
                Rule.ExpirationTime = tempExpiration.TimeOfDay;
            }

            this.DataContext = Rule;
        }

        private async void ApplyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Apply Button Pressed");
            await Rule.Save();

            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void CancelButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Cancel Button Pressed");

            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void NumberFilter(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            sender.Text = Regex.Replace(sender.Text, @"[^\d.-]", "");
        }
    }
}
