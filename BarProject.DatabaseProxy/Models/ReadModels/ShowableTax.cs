namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableTax
    {
        public string TaxName { get; set; }
        public double? TaxValue { get; set; }
        public int? Id { get; set; }
        public ShowableTax(DatabaseConnector.Tax tax)
        {
            TaxName = tax.tax_name;
            TaxValue = tax.tax_value;
            Id = tax.id;
        }
        public ShowableTax()
        {
            Id = null;
        }
    }
}
