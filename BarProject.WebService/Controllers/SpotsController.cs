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
    [RoutePrefix("api/spots")]
    public class SpotsController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [ResponseType(typeof(List<ShowableSpot>))]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(SpotsFunctions.GetSpots());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpPost, Route("")]
        [Authorize(Roles = "Admin,Owner")]
        public IHttpActionResult Post(ShowableSpot spot)
        {
            try
            {
                SpotsFunctions.AddSpot(spot);
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
                SpotsFunctions.RemoveSpot(id);
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
        public IHttpActionResult Patch(int id, [FromBody]ShowableSpot spot)
        {
            try
            {
                SpotsFunctions.UpdateSpot(id, spot);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
