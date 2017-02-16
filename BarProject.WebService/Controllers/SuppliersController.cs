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

    [RoutePrefix("api/suppliers")]
    public class SuppliersController : ApiController
    {
        [HttpGet, Route("")]
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(IEnumerable<ShowableSupplier>))]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(SuppliersFunctions.GetSuppliers());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpPost, Route("")]
        [Authorize(Roles = "Admin,Owner")]
        public IHttpActionResult Post(ShowableSupplier category)
        {
            try
            {
                SuppliersFunctions.AddSupplier(category);
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
                SuppliersFunctions.RemoveSuppliers(id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpPatch]
        [Authorize(Roles = "Admin,Owner")]
        [Route("{id}")]
        public IHttpActionResult Patch(int id, ShowableSupplier supplier)
        {
            try
            {
                SuppliersFunctions.UpdateSuppliers(id, supplier);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
}
