using System.Net;
using BarProject.DatabaseProxy.Models.ExceptionHandlers;
using BarProject.DatabaseProxy.Models.Utilities;

namespace BarProject.DatabaseProxy.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class CategoriesFunctions
    {
        public static List<ShowableCategory> GetAllCategories()
        {
            using (var db = new BarProjectEntities())
            {
                return (from data in db.Categories
                        orderby data.id
                        select data).ToAnotherType<Category, ShowableCategory>().ToList();
            }
        }
        public static List<ShowableCategory> GetMainCategories()
        {
            return GetSubCategories(null);
        }
        public static List<ShowableCategory> GetSubCategories(int? id)
        {
            using (var db = new BarProjectEntities())
            {
                return db.Categories.Where(x => x.overriding_category == id).ToAnotherType<Category, ShowableCategory>().ToList();
            }
        }


        public static void AddCategory(ShowableCategory cat)
        {
            using (var db = new BarProjectEntities())
            {
                int? overriding = null;
                if (cat.Overriding != "")
                {
                    var q = db.Categories.Where(x => x.category_name == cat.Overriding).Select(x => x).FirstOrDefault();
                    if (q != null)
                    {
                        overriding = q.id;
                    }
                }
                db.addCategory(cat.Name, cat.Slug, overriding);
            }
        }
        public static void RemoveCategory(int? id)
        {
            using (var db = new BarProjectEntities())
            {
                db.removeCategory(id);
            }
        }

        public static void UpdateCategory(int id, ShowableCategory category)
        {
            using (var db = new BarProjectEntities())
            {
                if (string.IsNullOrEmpty(category.Overriding))
                {
                    db.updateCategory(id, category.Slug, category.Name, null);
                }
                else
                {
                    var tmp = db.Categories.FirstOrDefault(x => x.category_name == category.Overriding);
                    if (tmp == null)
                    {
                        throw new ResponseException(HttpStatusCode.NotFound, "No such overriding category", Utilities.ExceptionType.User);
                    }
                    db.updateCategory(id, category.Slug, category.Name, tmp.id);
                }
            }
        }
    }
}
