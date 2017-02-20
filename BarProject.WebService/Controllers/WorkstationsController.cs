using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BarProject.DatabaseProxy.Models;

namespace BarProject.WebService.Controllers
{
    using System.Web.Http.Description;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ExceptionHandlers;
    using DatabaseProxy.Models.ReadModels;
    using DatabaseProxy.Models.Utilities;

    [RoutePrefix("api/workstations")]
    public class WorkstationsController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Warehouse")]
        [ResponseType(typeof(List<ShowableWorkstation>))]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(WorkstationsFunctions.GetWorkstations());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpPost, Route("")]
        [Authorize(Roles = "Admin,Owner")]
        public IHttpActionResult Post([FromBody]ShowableWorkstation workstation)
        {
            try
            {
                WorkstationsFunctions.AddWorkstation(workstation);
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
                WorkstationsFunctions.RemoveWorkstation(id);
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
        public IHttpActionResult Patch(int id, [FromBody]ShowableWorkstation location)
        {
            try
            {
                WorkstationsFunctions.UpdateWorkstation(id, location);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }




        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [Route("rights")]
        [ResponseType(typeof(List<ShowableWorkstationRights>))]
        public IHttpActionResult GetRights()
        {
            try
            {

                return Ok(WorkstationsFunctions.GetWorkstationsRights());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpPost, Route("{id}/rights")]
        [Authorize(Roles = "Admin,Owner")]
        public IHttpActionResult Post(int id, [FromBody]UserPrivileges up)
        {
            try
            {
                WorkstationsFunctions.AddWorkstationRights(id, up);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpPatch]
        [Authorize(Roles = "Admin,Owner")]
        [Route("{id}/rights")]
        public IHttpActionResult PatchRights(int id, [FromBody]UserPrivileges location)
        {
            try
            {
                //TODO : update workstation rights
                throw new NotImplementedException();
                // WorkstationsFunctions.UpdateWorkstationPrivlidges(id, location);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin,Owner")]
        [Route("{id}/rights")]
        public IHttpActionResult DeleteRights(int id, [FromBody]UserPrivileges location)
        {
            try
            {
                WorkstationsFunctions.RemoveWorkstationRights(id, location);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
