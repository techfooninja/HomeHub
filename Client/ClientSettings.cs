namespace HomeHub.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using HomeHub.Shared;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public static class ClientSettings
    {
        private static string[] _possibleTempFormats = Enum.GetNames(typeof(TemperatureScale));

        public static string[] PossibleTemperatureFormats
        {
            get
            {
                return _possibleTempFormats;
            }
        }

        public static TemperatureScale TemperatureFormat
        {
            get;
            set;
        }

        public static int RefreshInterval { get; set; }
    }
}
