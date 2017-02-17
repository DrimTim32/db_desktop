using System;
using System.Web.Http;
using BarProject.DatabaseProxy.Functions;
using BarProject.DatabaseProxy.Models.ExceptionHandlers;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.Utilities;

namespace BarProject.WebService.Controllers
{
    [RoutePrefix("api/locations")]
    public class OrdersController : ApiController
    {
        //[HttpPost, Route("")]
        //[Authorize(Roles = "Admin,Owner")]
        //public IHttpActionResult Post(ShowableOrder location)
        //{
        //    try
        //    {
        //        LocationsFunctions.AddLocation(location);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
        //    }
        //}
    }
}
