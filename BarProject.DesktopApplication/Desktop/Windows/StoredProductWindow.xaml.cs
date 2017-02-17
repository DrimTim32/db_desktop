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
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Threading;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ReadModels;
    using Library.RestHelpers;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for StoredProductWindow.xaml
    /// </summary>
    public partial class StoredProductWindow : MetroWindow
    {
        private object productIdLocker = new object();
        private object simpleproductLocker = new object();

        private int productId
        {
            get
            {
                lock (productIdLocker) return _productId;
            }
        }

        private ShowableStoredProduct _storedProduct;
        private readonly int _productId;

        public ShowableStoredProduct StoredProduct
        {
            get
            {
                lock (simpleproductLocker)
                {
                    if (_storedProduct == null)
                        _storedProduct = new ShowableStoredProduct();
                    return _storedProduct;
                }
            }
            set
            {
                lock (simpleproductLocker)
                    _storedProduct = value;
            }
        }
        public StoredProductWindow(int id)
        {
            _productId = id;
            Loaded += StoredProductWindow_Loaded; ;
            InitializeComponent();
            TextTaxName.SelectionChanged += TextTaxName_SelectionChanged;
        }
        private void TextTaxName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillTaxName();
        }
        private void FillTaxName()
        {

            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoFillTaxName));
        }
        private async void DoFillTaxName()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetTaxes();
            var firstOrDefault = tmp.Data.FirstOrDefault(x => x.TaxName == TextTaxName.Text);
            if (firstOrDefault != null)
                TextTaxValue.Text = firstOrDefault.TaxValue.ToString();
            else
                TextTaxValue.Text = "";
            ProgressBarStop();
        }
        private void StoredProductWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetSources();
        }
        private void GetSources()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetSources));
        }

        private async void DoGetSources()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetTaxes();
            TextTaxName.ItemsSource = tmp.Data.Select(x => x.TaxName);
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                ReloadModel();
            }));
        }

        private void ReloadModel()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetStoredProduct(productId);
            StoredProduct.LoadFromAnother(tmp.Data);
            ProgressBarStop();
        }
    }
}
