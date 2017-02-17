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

    [RoutePrefix("api/categories")]
    public class CategoriesController : ApiController
    {

        [HttpGet, Route("main")]
        [Authorize]
        [ResponseType(typeof(IEnumerable<ShowableCategory>))]
        public IHttpActionResult GetMain()
        {
            try
            {
                var categories = CategoriesFunctions.GetMainCategories();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }

        [HttpGet, Route("{id}/products")]
        [Authorize]
        [ResponseType(typeof(IEnumerable<ShowableCategory>))]
        public IHttpActionResult GetProducts(int id)
        {
            try
            {
                var categories = ProductsFunctions.GetByCategory(id);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpGet, Route("{id}")]
        [Authorize]
        [ResponseType(typeof(IEnumerable<ShowableCategory>))]
        public IHttpActionResult GetSubCategories(int id)
        {
            try
            {
                var categories = CategoriesFunctions.GetSubCategories(id);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }

        [HttpGet, Route("")]
        [Authorize]
        [ResponseType(typeof(IEnumerable<ShowableCategory>))]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(CategoriesFunctions.GetAllCategories());
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
        [HttpPost, Route("")]
        [Authorize(Roles = "Admin,Owner")]
        public IHttpActionResult Post(ShowableCategory category)
        {
            try
            {
                CategoriesFunctions.AddCategory(category);
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
                CategoriesFunctions.RemoveCategory(id);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ResponseException(ex, Utilities.ExceptionType.Unknown);
            }
        }
    }
}
