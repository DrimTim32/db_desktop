using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseProxy.Models.ReadModels;
using RestSharp;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    public partial class RestClient
    {
        public async Task<IRestResponse<List<ShowablePrices>>> GetPrices()
        {
            var request = new RestRequest("api/prices/", Method.GET);
            request.AddHeader("Authorization", $"bearer {token}");
            request.AddHeader("Content-Type", "application/json");
            var data = await client.ExecuteGetTaskAsync<List<ShowablePrices>>(request);
            return data;
        } 
    }
}
