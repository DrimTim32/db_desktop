using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowablePrices
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime PeriodStart { get; set; }
        public ShowablePrices() { }

        public ShowablePrices(productsLastPricesWithName prices)
        {
            ProductID = prices.product_id;
            Name = prices.name;
            Price = prices.price;
            PeriodStart = prices.period_start;
        }
    }
}
