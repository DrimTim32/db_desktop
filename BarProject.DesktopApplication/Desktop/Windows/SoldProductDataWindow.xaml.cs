using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using BarProject.DesktopApplication.Common.Utils;
using RestSharp;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    using System.Windows.Threading;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ReadModels;
    using Library.RestHelpers;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for SoldProductDataWindow.xaml
    /// </summary>
    public partial class SoldProductDataWindow : MetroWindow
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
            _productId = id;
            Loaded += SoldProductWindow_Loaded;
            InitializeComponent();
            TextTaxName.SelectionChanged += TextTaxName_SelectionChanged;
        }

        private void TextTaxName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillTaxName();
        }

        private void SoldProductWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetSources();
        }
        private void GetSources()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                TextTaxName.IsReadOnly = true;
            }));
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(DoGetSources));
        }

        private void FillTaxName()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(DoFillTaxName));
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
        private async void DoGetSources()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetTaxes();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                TextTaxName.ItemsSource = tmp.Data.Select(x => x.TaxName);
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(ReloadModel));
            }
        }
        private void ReloadModel()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(DoRefreshData));
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetSoldProduct(productId);
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                SoldProduct.LoadFromAnother(tmp.Data);
            }
            ProgressBarStop();
        }
        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        private void ShowPricesHistoryClick(object sender, RoutedEventArgs e)
        {
            var window = new PricesHistory(_productId);
            window.ShowDialog();
        }
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var message = "";
            if (string.IsNullOrEmpty(SoldProduct.CategoryName))
            {
                message = "You cannot add product with no category";
            }
            if (string.IsNullOrEmpty(SoldProduct.TaxName))
            {
                message = "You cannot add product with no tax";
            }
            if (string.IsNullOrEmpty(SoldProduct.UnitName))
            {
                message = "You cannot add product with no unit";
            }
            if (TextPrice.Value == null)
            {
                message = "Sold product must have price!";
            }
            if (message != "")
            {
                MessageBoxesHelper.ShowWindowInformation("Problem with product", message);
                return;
            }
            SoldProduct.Price = TextPrice.Value;
            RestClient.Client().UpdateProduct(this.SoldProduct, ((response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                else
                {
                    MessageBox.Show("Success");
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(Close));
                }
            }));
        }
    }
}
