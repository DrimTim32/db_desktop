using System;
using System.Net;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using Microsoft.Owin;
using System.Linq;
using System.Net.Http;
namespace BarProject.WebService.Controllers
{
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Web.Http.Description;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models;
    using DatabaseProxy.Models.ExceptionHandlers;
    using DatabaseProxy.Models.ReadModels;
    using DatabaseProxy.Models.Utilities;
    using DatabaseProxy.Models.WriteModels;

    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(IEnumerable<ShowableUser>))]
        [Route("")]
        public IHttpActionResult Get()
        {
            var claims = Request.GetOwinContext().Authentication.User.Claims;
            var role = UserPrivlidgesExtensions.GetValueFromDescription(claims.First(x => x.Type == ClaimTypes.Role).Value);
            if (role == UserPrivileges.Admin || role == UserPrivileges.Owner)
                return Ok(UserFunctions.GetAllUsers());
            return Unauthorized();
        }

        /// <summary>
        /// Returns user's permissions
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(UserPrivileges))]
        [Route("privileges/{username}")]
        public IHttpActionResult Get(string username)
        {
            var claims = Request.GetOwinContext().Authentication.User.Claims;
            var claimsArray = claims as Claim[] ?? claims.ToArray();
            var role = UserPrivlidgesExtensions.GetValueFromDescription(claimsArray.First(x => x.Type == ClaimTypes.Role).Value);
            if (role == UserPrivileges.Admin || role == UserPrivileges.Owner)
                return Ok(UserFunctions.GetPrivileges(username));
            var firstOrDefault = claimsArray.FirstOrDefault(x => x.Type == "username");
            if (firstOrDefault != null && username == firstOrDefault.Value)
            {
                return Ok(UserFunctions.GetPrivileges(username));
            }
            return Unauthorized();
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPost, Route("")]
        public IHttpActionResult Post(WritableUser user)
        {
            try
            {
                UserFunctions.AddUser(user);
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Database, user);
            }
            return Ok();
        }
        //TODO : remove user
        //[HttpDelete]
        //[Authorize(Roles = "Admin,Owner")]
        //[Route("{username}")]
        //public IHttpActionResult Remove(string username)
        //{
        //    try
        //    {
        //        UserFunctions.Remove(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ResponseException(ex, Utilities.ExceptionType.Database, user);
        //    }
        //    return Ok();
        //}
    }

}
