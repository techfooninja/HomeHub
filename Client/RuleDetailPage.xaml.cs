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
        private bool _isNewRule = false;
        private bool _isOverride = false;
        private RuleProxy _rule;

        public RuleDetailPage()
        {
            this.InitializeComponent();
        }

        public DateTimeOffset ExpirationDate { get; set; }
        public TimeSpan ExpirationTime { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
            {
                _isNewRule = true;
                _rule = new RuleProxy();
            }
            else
            {
                TransitionInfo info = (TransitionInfo)e.Parameter;
                _rule = (RuleProxy)info.Rule ?? new RuleProxy();
                _isOverride = info.IsOverride;
                _isNewRule = info.Rule == null;
            }

            if (_isOverride)
            {
                Override.Visibility = Visibility.Visible;
                Default.Visibility = Visibility.Collapsed;
            }
            else
            {
                Override.Visibility = Visibility.Collapsed;
                Default.Visibility = Visibility.Visible;
            }

            ExpirationDate = DateTime.Now.Date;
            ExpirationTime = DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0));

            LowTempUnit.Text = AppSettings.Instance.TemperatureFormatUnit;
            HighTempUnit.Text = AppSettings.Instance.TemperatureFormatUnit;

            this.DataContext = _rule;
            Override.DataContext = this;
        }

        private async void ApplyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("Apply Button Pressed");

            string response = String.Empty;
            bool isSuccessful = false;
            HttpResponseMessage responseMsg;

            if (_isNewRule)
            {
                Debug.WriteLine("Adding new rule");

                Uri uri;

                if (_isOverride)
                {
                    uri = new Uri(String.Format("http://192.168.4.181:8800/api/thermostat/setholdtemp?expiration={0}", ExpirationDate.Add(ExpirationTime).ToString("o")));
                }
                else
                {
                    uri = new Uri("http://192.168.4.181:8800/api/thermostat/addrule");
                }
                
                HttpClient client = new HttpClient();
                var jsonContent = JsonConvert.SerializeObject(_rule);
                using (var content = new HttpStringContent(jsonContent, Windows.Storage.Streams.UnicodeEncoding.Utf8))
                {
                    responseMsg = await client.PutAsync(uri, content);
                }
            }
            else
            {
                Debug.WriteLine("Updating existing rule");
                Uri uri = new Uri("http://192.168.4.181:8800/api/thermostat/updaterule");
                HttpClient client = new HttpClient();
                var jsonContent = JsonConvert.SerializeObject(_rule);
                using (var content = new HttpStringContent(jsonContent, Windows.Storage.Streams.UnicodeEncoding.Utf8))
                {
                    responseMsg = await client.PostAsync(uri, content);
                }
            }

            response = await responseMsg.Content.ReadAsStringAsync();
            isSuccessful = responseMsg.IsSuccessStatusCode;

            if (isSuccessful && Frame.CanGoBack)
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
