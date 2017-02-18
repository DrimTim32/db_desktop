namespace BarProject.DesktopApplication.Library.RestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DatabaseProxy.Models.ReadModels;
    using RestSharp;
    using RestSharp.Serializers;

    public partial class RestClient
    {

        public async Task<IRestResponse<List<ShowableCategory>>> GetMainCategories()
        {
            var request = new RestRequest("api/categories/main/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableCategory>>(request);
            return data;
        }
        public async Task<IRestResponse<List<ShowableCategory>>> GetSubCategoriesAsync(int id)
        {
            var request = new RestRequest($"api/categories/{id}", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableCategory>>(request);
            return data;
        }
        public async Task<IRestResponse<List<ShowableCategory>>> GetCategories()
        {
            var request = new RestRequest("api/categories/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableCategory>>(request);
            return data;
        }
        public void RemoveCategory(int? id, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/categories/{id}", Method.DELETE);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            client.ExecuteAsync(request, callback);
        }
        public void AddCategory(ShowableCategory category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/categories/", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
        public void UpdateCategory(ShowableCategory category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/categories/{category.Id}", Method.PATCH) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
    }
}
