using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using BarProject.DatabaseProxy.Functions;
using BarProject.DatabaseProxy.Models.ExceptionHandlers;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.Utilities;
using BarProject.DatabaseProxy.Models.WriteModels;

namespace BarProject.WebService.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        [HttpPost, Route("")]
        [Authorize]
        public IHttpActionResult Post(WritableOrder location)
        {
            try
            {
                var claims = Request.GetOwinContext().Authentication.User.Claims;
                var first = claims.First(x => x.Type == "username");

                OrdersFuncstions.AddOrder(first.Value, location);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
