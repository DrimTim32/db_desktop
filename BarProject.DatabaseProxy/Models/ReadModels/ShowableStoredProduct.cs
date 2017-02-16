using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Annotations;
    using DatabaseConnector;

    public class ShowableStoredProduct : INotifyPropertyChanged
    {
        private string _name;
        private string _categoryName;
        private string _unitName;
        private string _taxName;
        private double _taxValue;

        public string Name
        {
            get { return _name; }
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
                OnPropertyChanged(nameof(TaxValue));
            }
        }


        public ShowableStoredProduct() { }

        public ShowableStoredProduct(productDetails_Result product)
        {
            Name = product.name;
            CategoryName = product.category_name;
            UnitName = product.unit_name;
            TaxName = product.tax_name;
            TaxValue = product.tax_value;
        }

        public void LoadFromAnother(ShowableStoredProduct product)
        {
            Name = product.Name;
            CategoryName = product.CategoryName;
            UnitName = product.UnitName;
            TaxName = product.TaxName;
            TaxValue = product.TaxValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
