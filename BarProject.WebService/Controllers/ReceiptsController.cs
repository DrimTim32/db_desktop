using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BarProject.DatabaseProxy.Functions;
using BarProject.DatabaseProxy.Models.ExceptionHandlers;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.Utilities;

namespace BarProject.WebService.Controllers
{
    [RoutePrefix("api/receipts")]
    public class ReceiptsController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(IEnumerable<ShowableReceipt>))]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(RecipiesFunctions.GetRecipts());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPost, Route("")]
        public IHttpActionResult Post(ShowableReceipt tax)
        {
            try
            {
                RecipiesFunctions.AddRecipt(tax);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner")]
        [HttpPatch, Route("{id}")]
        public IHttpActionResult Patch(int id, [FromBody] ShowableReceipt tax)
        {
            try
            {
                RecipiesFunctions.UpdateRecipt(id, tax);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner")]
        [HttpDelete, Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                RecipiesFunctions.RemoveRecipt(id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
