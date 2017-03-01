namespace HomeHub.Shared
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Windows.Networking.Connectivity;
    using Windows.Web.Http;
    using Windows.Web.Http.Filters;

    public enum RequestType
    {
        Get,
        Post,
        Put,
        Delete
    }

    public static class NetworkHelpers
    {
        private static HttpClient _client = null;
        private static object _lock = new object();

        public static string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return null;

            if (icp.IsWlanConnectionProfile)
            {
                var hostname =
                    NetworkInformation.GetHostNames()
                        .SingleOrDefault(
                            hn =>
                                hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                == icp.NetworkAdapter.NetworkAdapterId);

                return hostname?.CanonicalName;
            }

            return null;
        }

        public static string GetLocalDomainName()
        {
            return Dns.GetHostName();
        }

        public static async Task<HttpResponseMessage> SendRequest(RequestType type, Uri uri, string content)
        {
            lock (_lock)
            {
                if (_client == null)
                {
                    var filter = new HttpBaseProtocolFilter();
                    filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.NoCache;
                    _client = new HttpClient(filter);
                }
            }

            HttpStringContent httpContent = null;

            if (content != null)
                httpContent = new HttpStringContent(content, Windows.Storage.Streams.UnicodeEncoding.Utf8);

            HttpResponseMessage response = null;

            try
            {
                switch (type)
                {
                    case RequestType.Get: response = await _client.GetAsync(uri); break;
                    case RequestType.Post: response = await _client.PostAsync(uri, httpContent); break;
                    case RequestType.Put: response = await _client.PutAsync(uri, httpContent); break;
                    case RequestType.Delete: response = await _client.DeleteAsync(uri); break;
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

        public static async Task<string> Ping(string host)
        {
            try
            {
                IPHostEntry he = await Dns.GetHostEntryAsync(host);
                Debug.WriteLine("Found: " + he.HostName);
                return he.HostName;
            }
            catch
            {
                return null;
            }
        }
    }
}
