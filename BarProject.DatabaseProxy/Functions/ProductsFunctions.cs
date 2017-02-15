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

        public static List<ShowableSoldProduct> GetSoldProductsView(int id)
        {
            using (var db = new BarProjectEntities())
            {
                return db.soldProductDetails(id).ToAnotherType<soldProductDetails_Result, ShowableSoldProduct>().ToList();
            }
        }

        public static List<ShowablePricesHistory> GetPricesHistory(int id)
        {
            using (var db = new BarProjectEntities())
            {
                return db.pricesHistory(id).ToAnotherType<pricesHistory_Result, ShowablePricesHistory>().ToList();
            }

        }
        public static IEnumerable<ShowableSimpleProduct> GetStoredProdcuts(int id)
        {
            using (var db = new BarProjectEntities())
            {
              return db.productDetails(id).ToAnotherType<productDetails_Result, ShowableSimpleProduct>().ToList();
            }

        }
    }
}
