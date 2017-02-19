using System; 
using BarProject.DatabaseProxy.Models.WriteModels;
using Newtonsoft.Json;
using RestSharp;
using JsonSerializer = RestSharp.Serializers.JsonSerializer;

namespace BarProject.DesktopApplication.Library.RestHelpers
{
    public partial class RestClient
    {
        public void AddOrder(WritableOrder order, Action<IRestResponse, RestRequestAsyncHandle> callback)
        {
            var request = new RestRequest("api/orders/", Method.POST);
            request.AddHeader("Authorization", $"bearer {token}");  
            var json = JsonConvert.SerializeObject(order);
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;   
            client.ExecuteAsync(request, callback);
        }
    }
}
