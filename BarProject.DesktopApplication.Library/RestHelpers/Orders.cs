﻿using System;
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
        public void AddUserOrder(WritableOrder order, Action<IRestResponse, RestRequestAsyncHandle> callback)
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
            var request = new RestRequest($"api/warehouse/orders/{id}", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableWarehouseOrderDetails>>(request);
            return data;
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
    }
}
