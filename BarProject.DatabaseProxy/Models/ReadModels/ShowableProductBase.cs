using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BarProject.DatabaseProxy.Models.ReadModels
{
    public class ShowableProductBase : INotifyPropertyChanged
    {
        public ShowableProductBase()
        {
            Id = null;
        }
        private string _name;
        private string _categoryName;
        private string _unitName;
        private string _taxName;
        private double _taxValue;
        public decimal? Price { get; set; }
        public int? Id { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                OnPropertyChanged();
            }
        }

        public string UnitName
        {
            get { return _unitName; }
            set
            {
                _unitName = value;
                OnPropertyChanged();
            }
        }

        public string TaxName
        {
            get { return _taxName; }
            set
            {
                _taxName = value;
                OnPropertyChanged();
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}