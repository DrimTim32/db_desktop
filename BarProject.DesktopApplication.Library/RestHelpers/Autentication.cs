using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseProxy.Models;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    using System.Text.RegularExpressions;
    using RestSharp;
    using RestSharp.Authenticators;

    public partial class RestClient
    {
        static Regex tokenRegex = new Regex("\"access_token\":\"(.*?)\"", RegexOptions.Multiline);
        public void AutenticateMe(string login, string password, Action<IRestResponse> callback)
        {
            client.Authenticator = new HttpBasicAuthenticator(login, password);
            var request = new RestRequest("token", Method.POST);
            string encodedBody = $"grant_type=password&username={login}&password={password}";
            request.AddParameter("application/x-www-form-urlencoded", encodedBody, ParameterType.RequestBody);
            request.AddParameter("Content-Type", "application/x-www-form-urlencoded", ParameterType.HttpHeader);
            //request.Timeout = 7000;
            client.ExecuteAsync(request, resp =>
            {
                var tmp = resp.Content;
                if (!tmp.Contains("token"))
                    token = null;
                var match = tokenRegex.Match(tmp);
                token = match.Success ? match.Groups[1].Value : null;
                callback(resp);
            });



        }
        public string BaseUrl { get; set; }

    }
}
