using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.WriteModels;
using Newtonsoft.Json;
using RestSharp;
using JsonSerializer = RestSharp.Serializers.JsonSerializer;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    public partial class RestClient
    {
        public async Task<IRestResponse<List<ShowableClientOrder>>> GetClientOrders()
        {
            var request = new RestRequest("api/orders/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            var data = await client.ExecuteGetTaskAsync<List<ShowableClientOrder>>(request);
            return data;
        }
        public async Task<IRestResponse<List<ShowableClientOrderDetails>>> GetClientOrdersDetails(int id)
        {
            var request = new RestRequest($"api/orders/{id}/details", Method.GET); 
            request.AddHeader("Authorization", $"bearer {token}");
            var data = await client.ExecuteGetTaskAsync<List<ShowableClientOrderDetails>>(request);
            return data;
        }
        public void AddUserOrder(WritableOrder order, Action<IRestResponse<int>, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/orders/", Method.POST);
            request.AddHeader("Authorization", $"bearer {token}");
            var json = JsonConvert.SerializeObject(order);
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            client.ExecuteAsync(request, callback);
        }
        public async Task<IRestResponse<List<ShowableWarehouseOrder>>> GetWarehouseOrders()
        {
            var request = new RestRequest("api/warehouse/orders", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableWarehouseOrder>>(request);
            return data;
        }
        public async Task<IRestResponse<List<ShowableWarehouseOrderDetails>>> GetWarehouseOrdersDetails(int id)
        {
            var request = new RestRequest($"api/warehouse/orders/{id}/details", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableWarehouseOrderDetails>>(request);
            return data;
        }
        public void AddWarehouseOrderDetails(int orderId, ShowableWarehouseOrderDetails order, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/warehouse/orders/{orderId}/details", Method.POST);
            request.AddHeader("Authorization", $"bearer {token}");
            var json = JsonConvert.SerializeObject(order);
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            client.ExecuteAsync(request, callback);
        }
        public void UpdateWarehouseOrderDetails(int orderId, ShowableWarehouseOrderDetails order, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/warehouse/orders/{orderId}/details", Method.PATCH) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(order);
            client.ExecuteAsync(request, callback);
        }
        public void UpdateWarehouseOrder(ShowableWarehouseOrder order, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/warehouse/orders/{order.Id}", Method.PATCH) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(order);
            client.ExecuteAsync(request, callback);
        }
        public void AddWarehouseOrder(ShowableWarehouseOrder order, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/warehouse/orders/", Method.POST);
            request.AddHeader("Authorization", $"bearer {token}");
            var json = JsonConvert.SerializeObject(order);
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            client.ExecuteAsync(request, callback);
        }

        public void MarkPaid(int id, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/orders/mark/{id}", Method.POST);
            request.AddHeader("Authorization", $"bearer {token}");
            request.RequestFormat = DataFormat.Json;
            client.ExecuteAsync(request, callback);
        }
    }
}
