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
        public async Task<IRestResponse<List<ShowableUnit>>> GetUnits()
        {
            var request = new RestRequest("api/units/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowableUnit>>(request);
            return data;
        }
        public void RemoveUnit(int? id, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest($"api/units/{id}", Method.DELETE);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            client.ExecuteAsync(request, callback);
        }
        public void AddUnit(ShowableUnit category, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/units/", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"bearer {token}");
            request.JsonSerializer = new JsonSerializer();
            request.AddBody(category);
            client.ExecuteAsync(request, callback);
        }
    }
}
