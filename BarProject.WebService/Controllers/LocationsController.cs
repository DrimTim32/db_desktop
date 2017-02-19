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

    [RoutePrefix("api/locations")]
    public class LocationsController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [ResponseType(typeof(List<ShowableLocation>))]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(LocationsFunctions.GetLocations());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpPost, Route("")]
        [Authorize(Roles = "Admin,Owner")]
        public IHttpActionResult Post(ShowableLocation location)
        {
            try
            {
                LocationsFunctions.AddLocation(location);
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
                LocationsFunctions.RemoveLocation(id);
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
        public IHttpActionResult Patch(int id, [FromBody]ShowableLocation location)
        {
            try
            {
                LocationsFunctions.UpdateLocation(id, location);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
