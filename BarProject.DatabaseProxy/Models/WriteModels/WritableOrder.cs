using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BarProject.DatabaseProxy.Annotations;
using BarProject.DatabaseProxy.Models.ReadModels;

namespace BarProject.DatabaseProxy.Models.WriteModels
{
    public class WritableOrder : INotifyPropertyChanged
    {
        private decimal _sum = 0; 

        public WritableOrder()
        {
        }
         

        public void Clear()
        {
            _sum = 0;
            Details.Clear();
        }
        public void AddProduct(ShowableSoldProduct product, short quantity)
        {
            if (!product.Id.HasValue)
                throw new ArgumentException("Product ID must not be null.");
            var order = new WritableOrderDetails()
            {
                ProductId = product.Id.Value,
                Quantity = quantity,
                Price = product.Price
            };
            Details.Add(order);
            _sum += quantity * product.Price;
            OnPropertyChanged(nameof(Sum));
            OnPropertyChanged(nameof(Details));
        }
        public decimal Sum => _sum;

        public WritableOrderDetailsCollection Details { get; set; } = new WritableOrderDetailsCollection();


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
