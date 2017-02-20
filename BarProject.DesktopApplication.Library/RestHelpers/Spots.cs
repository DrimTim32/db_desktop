using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseProxy.Models.ReadModels;
using RestSharp;
using RestSharp.Serializers;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    public partial class RestClient
    {
        public async Task<IRestResponse<List<ShowableSpot>>> GetSpots()
        {
            var request = new RestRequest("api/spots/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableSpot>>(request);
            return data;
        }
        public void RemoveSpot(int? id, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/spots/{id}", Method.DELETE);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            client.ExecuteAsync(request, callback);
        }
        public void AddSpot(ShowableSpot category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/spots/", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
        public void UpdateSpot(ShowableSpot category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/spots/{category.Id}", Method.PATCH) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
    }
}
