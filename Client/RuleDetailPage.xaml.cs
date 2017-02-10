using HomeHub.Client.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeHub.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RuleDetailPage : Page
    {
        public RuleDetailPage()
        {
            this.InitializeComponent();
        }

        public ClientSettingsViewModel ClientSettings
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

            Rule.ExpirationDate = DateTime.Now.Date;
            Rule.ExpirationTime = DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0));
            this.DataContext = Rule;
        }

        private async void ApplyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Apply Button Pressed");
            Rule.Save();
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
