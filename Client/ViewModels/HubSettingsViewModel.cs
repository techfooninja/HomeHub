namespace HomeHub.Client.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HubSettingsViewModel : NotificationBase<HubSettings>
    {
        public HubSettingsViewModel(HubSettings settings = null) : base(settings)
        {

        }

        public int PollingTime
        {
            get { return This.PollingTime; }
            set { SetProperty(This.PollingTime, value, () => This.PollingTime = value); }
        }

        public int TargetBufferTime
        {
            get { return This.TargetBufferTime; }
            set { SetProperty(This.TargetBufferTime, value, () => This.TargetBufferTime = value); }
        }

        public bool UseRules
        {
            get { return This.UseRules; }
            set { SetProperty(This.UseRules, value, () => This.UseRules = value); }
        }
    }
}
