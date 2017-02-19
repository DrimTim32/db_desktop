using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BarProject.DatabaseProxy.Models.WriteModels;

namespace BarProject.WebService.Controllers
{
    using System.Web.Http.Description;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ExceptionHandlers;
    using DatabaseProxy.Models.ReadModels;
    using DatabaseProxy.Models.Utilities;

    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(IEnumerable<ShowableSimpleProduct>))]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(ProductsFunctions.GetProductView());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(IEnumerable<ShowableSimpleProduct>))]
        [Route("stored/{id}")]
        public IHttpActionResult GetStored(int id)
        {
            try
            {
                return Ok(ProductsFunctions.GetStoredProduct(id));
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(ShowableSoldProduct))]
        [Route("sold/{id}")]
        public IHttpActionResult GetSold(int id)
        {
            try
            {
                var tmp = ProductsFunctions.GetSoldProductData(id);
                return Ok(tmp);
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(List<ShowablePricesHistory>))]
        [Route("history/{id}")]
        public IHttpActionResult GetHistory(int id)
        {
            try
            {
                return Ok(ProductsFunctions.GetPricesHistory(id));
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner")]
        [HttpPatch, Route("{id}")]
        public IHttpActionResult UpdateProduct(int id, [FromBody]ShowableProductBase product)
        {
            try
            {
                ProductsFunctions.UpdateProduct(id, product);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner")]
        [HttpDelete, Route("{id}")]
        public IHttpActionResult UpdateProduct(int id)
        {
            try
            {
                ProductsFunctions.RemoveProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost, Route("")]
        public IHttpActionResult Post([FromBody]WritableProduct product)
        {
            try
            { 
                ProductsFunctions.AddProduct(product);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [Authorize(Roles = "Admin,Owner")]
        [ResponseType(typeof(List<string>))]
        [HttpGet, Route("orderable")]
        public IHttpActionResult GetProductsForOrder()
        {
            try
            {
                return Ok(ProductsFunctions.GetOrderableProductsNames());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
