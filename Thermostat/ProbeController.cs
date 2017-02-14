namespace HomeHub.Hub
{
    using Restup.Webserver.Attributes;
    using Restup.Webserver.Models.Contracts;
    using Restup.Webserver.Models.Schemas;
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.ApplicationModel;
    using Windows.Networking;
    using Windows.Networking.Connectivity;

    [RestController(InstanceCreationType.Singleton)]
    class ProbeController
    {
        [UriFormat("/probe")]
        public IGetResponse GetProbe()
        {
            Debug.WriteLine("GetProbe");

            // Get package info for the app
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            // Get hostname for this device
            string hostname = NetworkHelpers.GetLocalDomainName();
            
            return new GetResponse(
              GetResponse.ResponseStatus.OK,
              String.Format("{0} v{1}.{2}.{3}.{4}, Hostname:{5}", 
                package.DisplayName,
                version.Major,
                version.Minor,
                version.Build,
                version.Revision,
                hostname)
            );
        }
    }
}
