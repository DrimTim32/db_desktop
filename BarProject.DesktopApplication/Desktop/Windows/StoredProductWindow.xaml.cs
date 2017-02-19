using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using BarProject.DesktopApplication.Common.Utils;
using RestSharp;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    using System.Diagnostics;
    using System.Windows.Threading;
    using DatabaseProxy.Models.ReadModels;
    using Library.RestHelpers;
    using MahApps.Metro.Controls;

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
        public StoredProductWindow(int id, IEnumerable<string> taxNames, IEnumerable<string> categoriesNames, IEnumerable<string> unitsNames)
        {
            _productId = id;
            InitializeComponent();
            Loaded += (s, e) =>
            {
                TextTaxName.ItemsSource = taxNames;
                TextUnitName.ItemsSource = unitsNames;
                TextCategoryName.ItemsSource = categoriesNames;
                RefreshData();
            };
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
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                var firstOrDefault = tmp.Data.FirstOrDefault(x => x.TaxName == TextTaxName.Text);
                if (firstOrDefault != null)
                    TextTaxValue.Text = firstOrDefault.TaxValue.ToString();
                else
                    TextTaxValue.Text = "";
            }
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

        private void RefreshData()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetStoredProduct(productId);
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                StoredProduct.LoadFromAnother(tmp.Data);
            }
            ProgressBarStop();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var message = "";
            if (string.IsNullOrEmpty(StoredProduct.CategoryName))
            {
                message = "You cannot remove category from product";
            }
            if (string.IsNullOrEmpty(StoredProduct.TaxName))
            {
                message = "You cannot remove tax from product";
            }
            if (string.IsNullOrEmpty(StoredProduct.UnitName))
            {
                message = "You cannot remove unit from product";
            }
            if (message != "")
            { 
                MessageBoxesHelper.ShowWindowInformation("Problem with product", message);
                return;
            }
            RestClient.Client().UpdateProduct(this.StoredProduct, ((response, handle) =>
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
