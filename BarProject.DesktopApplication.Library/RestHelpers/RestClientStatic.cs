using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    using RestSharp.Deserializers;

    partial class RestClient
    {

        private readonly RestSharp.RestClient _client;
        private static string _token;
        private static RestClient _staticClient { get; set; }

        private static readonly object tokenLocker = new object();
        private static readonly object clientLocker = new object();
        private static readonly object staticClientLocker = new object();

        private RestSharp.RestClient client
        {
            get
            {
                lock (clientLocker)
                {
                    return _client;
                }
            }

        }

        private static string token
        {
            get
            {
                lock (tokenLocker)
                {
                    return _token;
                }
            }
            set
            {
                lock (tokenLocker)
                {
                    _token = value;
                }
            }
        }
        private RestClient(string url)
        {
            BaseUrl = url;
            lock (clientLocker)
            {
                _client = new RestSharp.RestClient(url);
            }
        }
        public static RestClient Client(string url)
        {
            lock (staticClientLocker)
            {
                if (_staticClient == null || _staticClient.BaseUrl != url)
                {
                    _staticClient = new RestClient(url);
                }
                return _staticClient;
            }

        }
        public static RestClient Client()
        {
            lock (staticClientLocker)
            {
                if (_staticClient == null)
                {
                    throw new NullReferenceException("Client was not initialised");
                }

                return _staticClient;
            }

        }
    }
}
