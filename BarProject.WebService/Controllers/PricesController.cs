using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BarProject.DatabaseConnector;
using BarProject.DatabaseProxy.Functions;
using BarProject.DatabaseProxy.Models.ExceptionHandlers;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.Utilities;

namespace BarProject.WebService.Controllers
{
    [RoutePrefix("api/prices")]
    public class PricesController : ApiController
    { 
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [ResponseType(typeof(List<ShowablePrices>))]
        [HttpGet, Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(PricesFunctions.GetPrices());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
