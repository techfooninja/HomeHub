// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace HomeHub.Hub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net.Http;
    using Windows.ApplicationModel.Background;
    using Restup.Webserver.Rest;
    using Restup.Webserver.Http;
    using Restup.Webserver.File;
    using Restup.Webserver.Attributes;
    using Restup.Webserver.Models.Schemas;
    using Restup.Webserver.Models.Contracts;
    using HomeHub.Shared;

    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;
        private Thermostat _thermostat;
        private HttpServer _httpServer;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            // Initialize Thermostat
            _thermostat = Thermostat.Instance;

            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<ThermostatController>();

            var configuration = new HttpServerConfiguration()
                .ListenOnPort(8800)
                .RegisterRoute("api", restRouteHandler)
                .EnableCors(); // allow cors requests on all origins
            var httpServer = new HttpServer(configuration);
            _httpServer = httpServer;
            await httpServer.StartServerAsync();
        }
    }
}
