namespace HomeHub.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Networking;
    using Windows.Networking.Connectivity;
    using Windows.Networking.Sockets;
    using Windows.Web.Http;

    public enum RequestType
    {
        Get,
        Post,
        Put,
        Delete
    }

    public static class NetworkHelpers
    {
        public static string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return null;
            var hostname =
                NetworkInformation.GetHostNames()
                    .SingleOrDefault(
                        hn =>
                            hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                            == icp.NetworkAdapter.NetworkAdapterId);

            return hostname?.CanonicalName;
        }

        public static string GetLocalDomainName()
        {
            return Dns.GetHostName();
            /*var icp = NetworkInformation.GetInternetConnectionProfile();
            HostName hostname = null;

            if (icp?.NetworkAdapter != null)
            {
                hostname = NetworkInformation.GetHostNames().FirstOrDefault(
                        hn =>
                            hn.Type == Windows.Networking.HostNameType.DomainName &&
                            hn.DisplayName.IndexOf(".local", StringComparison.OrdinalIgnoreCase) < 0
                        );
            }

            return hostname?.DisplayName;*/
        }

        public static async Task<HttpResponseMessage> SendRequest(RequestType type, Uri uri, string content)
        {
            HttpClient client = new HttpClient();
            HttpStringContent httpContent = null;

            if (content != null)
                httpContent = new HttpStringContent(content, Windows.Storage.Streams.UnicodeEncoding.Utf8);

            HttpResponseMessage response = null;

            try
            {
                switch (type)
                {
                    case RequestType.Get: response = await client.GetAsync(uri); break;
                    case RequestType.Post: response = await client.PostAsync(uri, httpContent); break;
                    case RequestType.Put: response = await client.PutAsync(uri, httpContent); break;
                    case RequestType.Delete: response = await client.DeleteAsync(uri); break;
                    default: break;
                }
            }
            catch (Exception)
            {
                // Do nothing, it failed
            }
            finally
            {
                if (httpContent != null)
                    httpContent.Dispose();
            }

            return response;
        }

        public static async Task<bool> Ping(string host)
        {
            try
            {
                IPHostEntry he = await Dns.GetHostEntryAsync(host);
                Debug.WriteLine("Found: " + he.HostName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
