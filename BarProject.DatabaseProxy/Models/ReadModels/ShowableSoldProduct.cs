namespace BarProject.DatabaseProxy.Models.ReadModels
{
    using System;
    using DatabaseConnector;

    public class ShowableSoldProduct : ShowableProductBase
    {
        private decimal _price;
        private DateTime _periodStart;
        private int? _recepitId;

        public int? RecepitId
        {
            get { return _recepitId; }
            set
            {
                _recepitId = value;
                OnPropertyChanged();
            }
        }

        public DateTime PeriodStart
        {
            get { return _periodStart; }
            set
            {
                _periodStart = value;
                OnPropertyChanged(nameof(PeriodStart));
            }
        }

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));

            }
        }
        public ShowableSoldProduct()
        { 
        }
        public ShowableSoldProduct(productDetails_Result product)
        {
            Id = product.id;
            Name = product.name;
            CategoryName = product.category_name;
            UnitName = product.unit_name;
            TaxName = product.tax_name;
            TaxValue = product.tax_value;
        }
        public ShowableSoldProduct(soldProductDetails_Result product)
        {
            Id = product.product_id;
            Name = product.name;
            CategoryName = product.category_name;
            UnitName = product.unit_name;
            TaxName = product.tax_name;
            TaxValue = product.tax_value;
            RecepitId = product.receipt_id;
            PeriodStart = product.period_start;
            Price = product.price;

        }
        public ShowableSoldProduct(productsByCategory_Result product)
        {
            Id = product.id;
            Price = product.price;
            Name = product.name;

        }

        public void LoadFromAnother(ShowableSoldProduct product)
        {
            Id = product.Id;
            Name = product.Name;
            CategoryName = product.CategoryName;
            UnitName = product.UnitName;
            TaxName = product.TaxName;
            TaxValue = product.TaxValue;
            RecepitId = product.RecepitId;
            PeriodStart = product.PeriodStart;
            Price = product.Price;
        }

    }
}
