using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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
        public void AddProduct(ShowableSoldProduct product, int quantity)
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

    public class WritableOrderDetails
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class WritableOrderDetailsCollection : IList<WritableOrderDetails>, INotifyCollectionChanged
    {
        private List<WritableOrderDetails> List { get; set; } = new List<WritableOrderDetails>();
        public IEnumerator<WritableOrderDetails> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(WritableOrderDetails item)
        {
            var elem = List.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (elem != null)
            {
                elem.Quantity += item.Quantity;
            }
            else
            {
                List.Add(item);
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }

        public void Clear()
        {
            List.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
        }

        public bool Contains(WritableOrderDetails item)
        {
            return List.Contains(item);
        }

        public void CopyTo(WritableOrderDetails[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        public bool Remove(WritableOrderDetails item)
        {
            var tmp = List.Remove(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            return tmp;
        }

        public int Count => List.Count;

        public bool IsReadOnly => false;
        public int IndexOf(WritableOrderDetails item)
        {
            return List.IndexOf(item);
        }

        public void Insert(int index, WritableOrderDetails item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public WritableOrderDetails this[int index]
        {
            get { return List[index]; }
            set { throw new NotSupportedException(); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
