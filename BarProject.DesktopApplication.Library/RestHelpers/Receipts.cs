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
        public async Task<IRestResponse<List<ShowableReceipt>>> GetReceipts()
        {
            var request = new RestRequest("api/receipts/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableReceipt>>(request);
            return data;
        }
        public void RemoveReceipt(int? id, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/receipts/{id}", Method.DELETE);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            client.ExecuteAsync(request, callback);
        }
        public void AddReceipt(ShowableReceipt tax, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/receipts/", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(tax);
            client.ExecuteAsync(request, callback);
        }
        public void UpdateReceipt(ShowableReceipt tax, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/receipts/{tax.Id}", Method.PATCH) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(tax);
            client.ExecuteAsync(request, callback);
        }
    }
}
