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
        static Regex UserPrivilegesRegex = new Regex("\"UserPrivileges\":\"(.*?)\"", RegexOptions.Multiline);
        public void AutenticateMe(string login, string password, Action<IRestResponse, UserPrivileges> callback)
        {
            client.Authenticator = new HttpBasicAuthenticator(login, password);
            var request = new RestRequest("token", Method.POST);
            string encodedBody = $"grant_type=password&username={login}&password={password}";
            request.AddParameter("application/x-www-form-urlencoded", encodedBody, ParameterType.RequestBody);
            request.AddParameter("Content-Type", "application/x-www-form-urlencoded", ParameterType.HttpHeader);
            request.Timeout = 10000;
            client.ExecuteAsync(request, resp =>
            {
                var tmp = resp.Content;
                var privlidges = UserPrivileges.NoUser;
                if (!tmp.Contains("token"))
                    token = null;
                else
                {
                    var match = tokenRegex.Match(tmp);
                    token = match.Success ? match.Groups[1].Value : null;
                    match = UserPrivilegesRegex.Match(tmp);
                    var privlidgesStr = match.Success ? match.Groups[1].Value : null;
                    if (UserPrivlidgesExtensions.PrivligesNames.ContainsValue(privlidgesStr))
                        privlidges = UserPrivlidgesExtensions.GetValueFromDescription(privlidgesStr);

                }
                callback(resp, privlidges);
            });



        }
        public string BaseUrl { get; set; }

    }
}
