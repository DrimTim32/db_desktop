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
        public async Task<IRestResponse<List<ShowableSimpleProduct>>> GetProductsByCategory(int id)
        {
            var request = new RestRequest($"api/categories/{id}/products", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableSimpleProduct>>(request);
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
    }
}
