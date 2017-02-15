namespace BarProject.Models.ReadModels
{
    using System;
    using DatabaseConnector;

    public class ShowablePricesHistory
    {
        public ShowablePricesHistory() { }

        public ShowablePricesHistory(pricesHistory_Result price)
        {
            Price = price.price;
            PeriodStart = price.period_start;

        }
        public decimal Price { get; set; }
        public DateTime PeriodStart { get; set; }
    }
}
