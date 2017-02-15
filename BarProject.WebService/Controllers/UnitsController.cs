using System.Web.Http;

namespace BarProject.WebService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Web.Http.Description;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models;
    using DatabaseProxy.Models.ExceptionHandlers;
    using DatabaseProxy.Models.ReadModels;
    using DatabaseProxy.Models.Utilities;

    [RoutePrefix("api/units")]
    public class UnitsController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(IEnumerable<ShowableUnit>))]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(UnitsFunctions.GetAllUnits());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        } 
        [Authorize(Roles = "Admin,Owner")] 
        [HttpPost, Route(""), ActionName("Add product as admin")]
        public IHttpActionResult Post([FromBody]ShowableUnit unit)
        {
            try
            {
                UnitsFunctions.AddUnit(unit);
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
                UnitsFunctions.RemoveUnit(id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
