namespace BarProject.DatabaseProxy.Models.ReadModels
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using DatabaseConnector;

    public class ShowableSoldProduct : INotifyPropertyChanged
    {
        private int? id;
        private decimal _price;
        private DateTime _periodStart;
        private int? _recepitId;
        private double _taxValue;
        private string _taxName;
        private string _unitName;
        private string _categoryName;
        private string _name;

        public int? Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }

        public string UnitName
        {
            get { return _unitName; }
            set
            {
                _unitName = value;
                OnPropertyChanged(nameof(UnitName));
            }
        }

        public string TaxName
        {
            get { return _taxName; }
            set
            {
                _taxName = value;
                OnPropertyChanged(nameof(TaxName));
            }
        }

        public double TaxValue
        {
            get { return _taxValue; }
            set
            {
                _taxValue = value;
                OnPropertyChanged();
            }
        }

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
            id = null;
        }
        public ShowableSoldProduct(productDetails_Result product)
        {
            id = product.id;
            Name = product.name;
            CategoryName = product.category_name;
            UnitName = product.unit_name;
            TaxName = product.tax_name;
            TaxValue = product.tax_value;
        }
        public ShowableSoldProduct(soldProductDetails_Result product)
        {
            id = product.product_id;
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
            id = product.id;
            Price = product.price;
            Name = product.name;

        }

        public void LoadFromAnother(ShowableSoldProduct product)
        {
            id = product.id;
            Name = product.Name;
            CategoryName = product.CategoryName;
            UnitName = product.UnitName;
            TaxName = product.TaxName;
            TaxValue = product.TaxValue;
            RecepitId = product.RecepitId;
            PeriodStart = product.PeriodStart;
            Price = product.Price;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
