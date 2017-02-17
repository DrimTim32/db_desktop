namespace BarProject.DatabaseProxy.Models.ReadModels
{
    using DatabaseConnector;

    public class ShowableSimpleProduct
    {
        public ShowableSimpleProduct()
        {

        }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public bool IsSold { get; set; }
        public bool IsStored { get; set; }
        public int Id { get; set; }
        public ShowableSimpleProduct(productSimple product)
        {
            Name = product.name;
            CategoryName = product.category_name;
            IsSold = product.sold.Value;
            IsStored = product.stored.Value;
            Id = product.id;
        }

        public ShowableSimpleProduct(Product product)
        {
            Name = product.name;
            Id = product.id;
        }
    }
}
