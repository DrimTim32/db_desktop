﻿using System;
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

    [RoutePrefix("api/warehouse")]
    public class WarehouseController : ApiController
    {
        [Authorize(Roles = "Admin,Owner")]
        [HttpGet, ResponseType(typeof(List<ShowableWarehouseOrder>))]
        [Route("orders")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(WarehouseFunctions.GetWarehouseOrders());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost, ResponseType(typeof(List<ShowableWarehouseOrder>))]
        [Route("orders")]
        public IHttpActionResult Post([FromBody]ShowableWarehouseOrder order)
        {
            try
            {
                var claims = Request.GetOwinContext().Authentication.User.Claims;
                var first = claims.First(x => x.Type == "username");
                WarehouseFunctions.AddWarehouseOrder(first.Value,order);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner")]
        [HttpGet, ResponseType(typeof(List<ShowableWarehouseOrderDetails>))]
        [Route("details/{id}")]
        public IHttpActionResult GetDetails(int id)
        {
            try
            { 
                return Ok(WarehouseFunctions.GetWarehouseOrdersDetails(id));
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }

    }
}