namespace BarProject.Models.ReadModels
{
    public class ShowableTax
    {
        public string TaxName { get; set; }
        public double? TaxValue { get; set; }
        public int id;
        public ShowableTax(DatabaseConnector.Tax tax)
        {
            TaxName = tax.tax_name;
            TaxValue = tax.tax_value;
            id = tax.id;
        }
        public ShowableTax() { }
    }
}
