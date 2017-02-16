using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    using System.Diagnostics;
    using DatabaseProxy.Models.ReadModels;
    using RestSharp;
    using RestSharp.Serializers;

    public partial class RestClient
    {
        public async Task<IRestResponse<List<ShowableLocation>>> GetLocations()
        {
            var request = new RestRequest("api/locations/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableLocation>>(request);
            return data;
        }
        public void RemoveLocation(int? id, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/locations/{id}", Method.DELETE);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            client.ExecuteAsync(request, callback);
        }
        public void AddLocation(ShowableLocation category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/locations/", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
        public void UpdateLocation(ShowableLocation category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/locations/{category.Id}", Method.PATCH) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
    }
}
