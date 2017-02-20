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
        [Authorize(Roles = "Admin,Owner,Warehouse")]
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

        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [HttpPost, Route("")]
        public IHttpActionResult Post(ShowableReceipt receipt)
        {
            try
            {
                RecipiesFunctions.AddRecipt(receipt);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner,Warehouse")]
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
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [HttpGet, Route("{id}/details")]
        [ResponseType(typeof(IEnumerable<ShowableRecipitDetail>))]
        public IHttpActionResult GetDetails(int id)
        {
            try
            {
                return Ok(RecipiesFunctions.GetDetails(id));
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [HttpDelete, Route("{id}")]
        [ResponseType(typeof(IEnumerable<ShowableRecipitDetail>))]
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
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [HttpDelete, Route("{id}/details/{id2}")]
        public IHttpActionResult DeleteDetails(int id, int id2)
        {
            try
            {
                RecipiesFunctions.RemoveReciptDetails(id, id2);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [HttpPost, Route("{id}/details")]
        public IHttpActionResult PostDetails(int id, ShowableRecipitDetail receipt)
        {
            try
            {
                RecipiesFunctions.AddReciptDetails(id, receipt);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [HttpPatch, Route("{id}/details")]
        public IHttpActionResult Patch(int id, [FromBody] ShowableRecipitDetail tax)
        {
            try
            {
                RecipiesFunctions.UpdateReciptDetails(id, tax);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
