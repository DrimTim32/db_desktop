using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using DatabaseProxy.Models.ReadModels;
    using RestSharp;
    using RestSharp.Authenticators;

    public partial class RestClient
    {
        static Regex tokenRegex = new Regex("\"access_token\":\"(.*?)\"", RegexOptions.Multiline);
        public void AutenticateMe(string login, string password)
        {
            client.Authenticator = new HttpBasicAuthenticator(login, password);
            var request = new RestRequest("token", Method.POST);
            string encodedBody = $"grant_type=password&username={login}&password={password}";
            request.AddParameter("application/x-www-form-urlencoded", encodedBody, ParameterType.RequestBody);
            request.AddParameter("Content-Type", "application/x-www-form-urlencoded", ParameterType.HttpHeader);
            var tmp = client.Execute(request).Content;
            if (!tmp.Contains("token"))
                throw new ApplicationException("Problem a connecting to server");
            var match = tokenRegex.Match(tmp);
            if (match.Success)
                token = match.Groups[1].Value;
            else
                throw new HttpRequestException("Problem with token");
        }
        public string BaseUrl { get; set; }

    }
}
