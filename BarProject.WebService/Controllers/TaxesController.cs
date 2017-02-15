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

    [RoutePrefix("api/taxes")]
    public class TaxesController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(IEnumerable<ShowableTax>))]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(TaxesFunctions.GetAllTaxes());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPost, Route("")]
        public IHttpActionResult Post(ShowableTax tax)
        {
            try
            {
                TaxesFunctions.AddTax(tax);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin,Owner")]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                TaxesFunctions.RemoveTax(id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
