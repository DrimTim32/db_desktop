using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    using System.Windows.Threading;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ReadModels;
    using Library.RestHelpers;

    /// <summary>
    /// Interaction logic for SoldProductDataWindow.xaml
    /// </summary>
    public partial class SoldProductDataWindow : Window
    {
        private readonly object productIdLocker = new object();
        private readonly object soldProductLocker = new object();
        private readonly int _productId;

        private int productId
        {
            get
            {
                lock (productIdLocker) return _productId;
            }
        }
        private ShowableSoldProduct _soldProduct;
        public ShowableSoldProduct SoldProduct
        {
            get
            {
                lock (soldProductLocker)
                {
                    if (_soldProduct == null)
                        _soldProduct = new ShowableSoldProduct();
                    return _soldProduct;
                }
            }
            set
            {
                lock (soldProductLocker) _soldProduct = value;
            }
        }
        public SoldProductDataWindow(int id)
        {
            InitializeComponent();
            Loaded += SoldProductWindow_Loaded; ;
            _productId = id;
        }

        private void SoldProductWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadModel();
        }
        private void ReloadModel()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetSoldProduct(productId);
            SoldProduct.LoadFromAnother(tmp.Data);
            ProgressBarStop();
        }
        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
    }
}
