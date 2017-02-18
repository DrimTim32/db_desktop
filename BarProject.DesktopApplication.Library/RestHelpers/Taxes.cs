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
        public async Task<IRestResponse<List<ShowableTax>>> GetTaxes()
        {
            var request = new RestRequest("api/taxes/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableTax>>(request);
            return data;
        }
        public void RemoveTax(int? id, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/taxes/{id}", Method.DELETE);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            client.ExecuteAsync(request, callback);
        }
        public void AddTax(ShowableTax tax, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/taxes/", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(tax);
            client.ExecuteAsync(request, callback);
        }
        public void UpdateTax(ShowableTax tax, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/taxes/{tax.Id}", Method.PATCH) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(tax);
            client.ExecuteAsync(request, callback);
        }
    }
}
