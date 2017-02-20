using System;
using System.Data.Entity.Migrations;
using System.Net;
using BarProject.DatabaseProxy.Models.ExceptionHandlers;
using BarProject.DatabaseProxy.Models.Utilities;
using BarProject.DatabaseProxy.Models.WriteModels;

namespace BarProject.DatabaseProxy.Functions
{
    using System.Collections.Generic;
    using System.Linq;
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class ProductsFunctions
    {
        public static List<ShowableProductUsage> GetUsage()
        {
            using (var db = new Entities())
            {
                return db.ProductsUsages.ToAnotherType<ProductsUsage, ShowableProductUsage>().ToList();
            }
        }
        public static List<ShowableSimpleProduct> GetProductView()
        {
            using (var db = new Entities())
            {
                return db.productSimples.ToAnotherType<productSimple, ShowableSimpleProduct>().ToList();
            }
        }

        public static ShowableSoldProduct GetSoldProductData(int id)
        {
            using (var db = new Entities())
            {
                return db.soldProductDetails(id).ToAnotherType<soldProductDetails_Result, ShowableSoldProduct>().FirstOrDefault();
            }
        }

        public static List<ShowablePricesHistory> GetPricesHistory(int id)
        {
            using (var db = new Entities())
            {
                return db.pricesHistory(id).ToAnotherType<pricesHistory_Result, ShowablePricesHistory>().ToList();
            }

        }
        public static ShowableStoredProduct GetStoredProduct(int id)
        {
            using (var db = new Entities())
            {
                return db.productDetails(id).ToAnotherType<productDetails_Result, ShowableStoredProduct>().FirstOrDefault();
            }

        }

        public static List<ShowableSoldProduct> GetSoldableProductsByCategory(int id)
        {
            using (var db = new Entities())
            {
                return db.productsByCategory(id).ToAnotherType<productsByCategory_Result, ShowableSoldProduct>().ToList();
            }
        }
        public static void UpdateProduct(int id, ShowableProductBase product)
        {
            using (var db = new Entities())
            {
                var category = db.Categories.FirstOrDefault(x => x.category_name == product.CategoryName);
                var unit = db.Units.FirstOrDefault(x => x.unit_name == product.UnitName);
                var tax = db.Taxes.FirstOrDefault(x => x.tax_name == product.TaxName);
                db.updateProduct(id, category?.id, unit?.id, tax?.id, product.Name);
                if (product.Price != null)
                    db.updatePrice(product.Id, product.Price.Value);
            }
        }

        public static List<string> GetOrderableProductsNames()
        {
            using (var db = new Entities())
            {
                var soldIds = (from sold in db.ProductsSolds
                               where sold.recipe_id == null
                               select sold.id);

                var storedIds = (from stored in db.ProductsStoreds select stored.id);

                var union = soldIds.Concat(storedIds);

                return (from pr in db.Products
                        join un in union
                        on pr.id equals un
                        select pr.name
                ).ToList();
            }
        }

        public static void RemoveProduct(int id)
        {

            using (var db = new Entities())
            {
                db.removeProduct(id);
            }
        }
        public static void AddProduct(WritableProduct product)
        {
            using (var db = new Entities())
            {
                var unit = db.Units.FirstOrDefault(x => x.unit_name == product.UnitName);
                var tax = db.Taxes.FirstOrDefault(x => x.tax_name == product.TaxName);
                var category = db.Categories.FirstOrDefault(x => x.category_name == product.CategoryName);
                var prod = new Product
                {
                    category_id = category.id,
                    unit_id = unit.id,
                    tax_id = tax.id,
                    name = product.Name
                };
                db.Products.AddOrUpdate(prod);
                db.SaveChanges();
                int id = prod.id;

                if (product.IsStored)
                {
                    db.addStoredProduct(id);
                }
                if (product.IsSold)
                {
                    if (string.IsNullOrEmpty(product.RecipitDescription))
                        db.addSoldProduct(id, null);
                    else
                    {
                        var recip = db.Recipes.FirstOrDefault(x => x.description == product.RecipitDescription);
                        db.addSoldProduct(id, recip?.id);
                    }
                    db.updatePrice(id, product.Price.Value);
                }
            }
        }
    }
}
