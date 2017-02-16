using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BarProject.WebService.Controllers
{
    using System.Web.Http.Description;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ExceptionHandlers;
    using DatabaseProxy.Models.ReadModels;
    using DatabaseProxy.Models.Utilities;

    [RoutePrefix("api/prices")]
    public class PricesController : ApiController
    {
        //[HttpGet]
        //[Authorize(Roles = "Admin,Owner")]
        //[ResponseType(typeof(IEnumerable<ShowableSimpleProduct>))]
        //[Route("")]
        //public IHttpActionResult Get()
        //{
        //    try
        //    {
        //        return Ok(ProductsFunctions.GetProductView());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
        //    }
        //}
    }
}
