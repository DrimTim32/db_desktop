namespace BarProject.DatabaseProxy.Models.WriteModels
{
    public class WritableOrderDetails
    {
        public int ProductId { get; set; }
        public short Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}