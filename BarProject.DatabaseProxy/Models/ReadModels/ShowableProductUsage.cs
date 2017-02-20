using BarProject.DatabaseConnector;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableProductUsage
    {
        public string ProductName { get; set; }
        public System.DateTime Date { get; set; }
        public double Quantity { get; set; }
        public ShowableProductUsage() { }

        public ShowableProductUsage(ProductsUsage usage)
        {
            ProductName = usage.Product.name;
            Date = usage.date;
            Quantity = usage.quantity;
        }
    }
}
