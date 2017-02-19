using System.Web.Http;

namespace BarProject.WebService.Controllers
{
    using System;
    using System.Collections.Generic;  
    using System.Web.Http.Description;
    using DatabaseProxy.Functions; 
    using DatabaseProxy.Models.ExceptionHandlers;
    using DatabaseProxy.Models.ReadModels;
    using DatabaseProxy.Models.Utilities;

    [RoutePrefix("api/units")]
    public class UnitsController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [ResponseType(typeof(IEnumerable<string>))]
        [Route("types")]
        public IHttpActionResult GetTypes()
        {
            try
            {
                return Ok(UnitsFunctions.GetTypes());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Warehouse")]
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
        [Authorize(Roles = "Admin,Owner,Warehouse")]
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
        [Authorize(Roles = "Admin,Owner,Warehouse")]
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
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [HttpPatch, Route("{id}")]
        public IHttpActionResult Patch(int id, ShowableUnit unit)
        {
            try
            {
                UnitsFunctions.UpdateUnit(id, unit);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
