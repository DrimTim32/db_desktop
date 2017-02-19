using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Functions
{
    using DatabaseConnector;
    using Extensions;
    using Models.ReadModels;

    public static class PricesFunctions
    {
        public static List<ShowablePrices> GetPrices()
        {
            using (var db = new Entities())
            {
                return db.productsLastPricesWithNames.Select(x => x).ToAnotherType<productsLastPricesWithName,ShowablePrices>().ToList();
            }
        }
    }
}
