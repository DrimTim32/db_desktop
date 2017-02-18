using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    using DatabaseProxy.Models.ReadModels;
    using RestSharp;
    using RestSharp.Serializers;

    public partial class RestClient
    {
        public async Task<IRestResponse<List<ShowableSimpleProduct>>> GetProducts()
        {
            var request = new RestRequest($"api/products/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableSimpleProduct>>(request);
            return data;
        }
        public async Task<IRestResponse<List<ShowableSoldProduct>>> GetProductsByCategory(int id)
        {
            var request = new RestRequest($"api/categories/{id}/products", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableSoldProduct>>(request);
            return data;
        }
        public async Task<IRestResponse<ShowableStoredProduct>> GetStoredProduct(int id)
        {
            var request = new RestRequest($"api/products/stored/{id}", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<ShowableStoredProduct>(request);
            return data;
        }
        public async Task<IRestResponse<ShowableSoldProduct>> GetSoldProduct(int id)
        {
            var request = new RestRequest($"api/products/sold/{id}", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new JsonSerializer();
            var task = client.ExecuteGetTaskAsync<ShowableSoldProduct>(request);
            var data = await task;
            return data;
        }

        public void GetPricesHistory(int id, Action<IRestResponse<List<ShowablePricesHistory>>, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/products/history/{id}", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            request.JsonSerializer = new JsonSerializer();
            client.ExecuteAsync(request, callback);
        }
        public void UpdateProduct(ShowableProductBase product, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/products/{product.Id}", Method.PATCH) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(product);
            client.ExecuteAsync(request, callback);
        }
    }
}
