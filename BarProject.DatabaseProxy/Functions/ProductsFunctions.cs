namespace BarProject.DatabaseProxy.Functions
{
    using System.Collections.Generic;
    using System.Linq;
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class ProductsFunctions
    {
        public static List<ShowableSimpleProduct> GetProductView()
        {
            using (var db = new BarProjectEntities())
            {
                return db.productSimples.ToAnotherType<productSimple, ShowableSimpleProduct>().ToList();
            }
        }

        public static ShowableSoldProduct GetSoldProductData(int id)
        {
            using (var db = new BarProjectEntities())
            {
                return db.soldProductDetails(id).ToAnotherType<soldProductDetails_Result, ShowableSoldProduct>().FirstOrDefault();
            }
        }

        public static List<ShowablePricesHistory> GetPricesHistory(int id)
        {
            using (var db = new BarProjectEntities())
            {
                return db.pricesHistory(id).ToAnotherType<pricesHistory_Result, ShowablePricesHistory>().ToList();
            }

        }
        public static ShowableStoredProduct GetStoredProduct(int id)
        {
            using (var db = new BarProjectEntities())
            {
                return db.productDetails(id).ToAnotherType<productDetails_Result, ShowableStoredProduct>().FirstOrDefault();
            }

        }

        public static List<ShowableSoldProduct> GetSoldableProductsByCategory(int id)
        {
            using (var db = new BarProjectEntities())
            {
                return db.productsByCategory(id).ToAnotherType<productsByCategory_Result, ShowableSoldProduct>().ToList();
            }
        }
        public static void UpdateProduct(int id, ShowableProductBase product)
        {
            using (var db = new BarProjectEntities())
            {
                var category = db.Categories.FirstOrDefault(x => x.category_name == product.CategoryName);
                var unit = db.Units.FirstOrDefault(x => x.unit_name == product.UnitName);
                var tax = db.Taxes.FirstOrDefault(x => x.tax_name == product.TaxName);
                db.updateProduct(id, category?.id, unit?.id, tax?.id, product.Name);
            }
        } 
    }
}
