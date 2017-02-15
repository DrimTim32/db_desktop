using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    using System.Collections;
    using DatabaseProxy.Models;
    using DatabaseProxy.Models.ReadModels;
    using DatabaseProxy.Models.WriteModels;
    using RestSharp;
    using RestSharp.Serializers;

    public partial class RestClient
    {
        public async Task<IRestResponse<UserPrivileges>> GetUserCredentials(string username)
        {
            var request = new RestRequest($"api/users/privileges/{username}", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<UserPrivileges>(request);
            return data;
        }

        public async Task<IRestResponse<List<ShowableUser>>> GetUsers()
        {
            var request = new RestRequest($"api/users/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableUser>>(request);
            return data;
        }
        public void AddUser(PureWritableUser user, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/users/", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");  
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(user);
            client.ExecuteAsync(request, callback);
        }
    }
}
