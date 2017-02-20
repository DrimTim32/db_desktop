using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
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

                var id = OrdersFuncstions.AddOrder(first.Value, location);
                return Ok(id);
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpPost, Route("mark/{id}")]
        [Authorize]
        public IHttpActionResult MarkPaid(int id)
        {
            try
            {
                OrdersFuncstions.MarkPaid(id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpGet, Route(""), Authorize]
        [ResponseType(typeof(IEnumerable<ShowableClientOrder>))]
        public IHttpActionResult GetOrders()
        {
            try
            {
                return Ok(OrdersFuncstions.GetOrders());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpGet, Route("{id}/details"), Authorize]
        [ResponseType(typeof(IEnumerable<ShowableClientOrder>))]
        public IHttpActionResult GetOrdersDetails(int id)
        {
            try
            {
                return Ok(OrdersFuncstions.GetDetails(id));
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }

    }
}
