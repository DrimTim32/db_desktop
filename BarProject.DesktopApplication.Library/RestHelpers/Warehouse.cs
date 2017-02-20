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
        public async Task<IRestResponse<List<ShowableWarehouseItem>>> GetWarehouse()
        {
            var request = new RestRequest("api/warehouse", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            var data = await client.ExecuteGetTaskAsync<List<ShowableWarehouseItem>>(request);
            return data;
        }
        public void RemoveWarehouse(ShowableWarehouseItem category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/warehouse/", Method.DELETE);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
        public void AddWarehouse(ShowableWarehouseItem category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/warehouse/", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
        public void UpdateWarehouse(ShowableWarehouseItem category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/warehouse/", Method.PATCH) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
    }
}
