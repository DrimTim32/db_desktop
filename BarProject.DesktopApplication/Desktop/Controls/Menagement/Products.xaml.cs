using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using BarProject.DesktopApplication.Common.Utils;
using MahApps.Metro.Controls.Dialogs;
using RestSharp;

namespace BarProject.DesktopApplication.Desktop.Controls.Menagement
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Windows;
    using DatabaseProxy.Models.ReadModels;
    using Library.RestHelpers;

    public partial class Products : UserControl
    {

        public List<string> UnitNames { get; set; }
        public List<string> TaxesNames { get; set; }
        public List<string> CategoriesNames { get; set; }
        public List<string> RecipiesNames { get; set; }
        private readonly object counterLocker = new object();
        private readonly SafeCounter _counter = new SafeCounter();
        public SafeCounter Counter
        {
            get
            {
                lock (counterLocker)
                {
                    return _counter;
                }
            }
        }
        private ObservableCollection<ShowableSimpleProduct> _productsList;
        private readonly object ShowableSimpleProductLock = new object();

        public ObservableCollection<ShowableSimpleProduct> ProductLists
        {
            get
            {
                lock (ShowableSimpleProductLock)
                {
                    if (_productsList == null)
                        _productsList =
                            new ObservableCollection<ShowableSimpleProduct>();
                    return _productsList;
                }
            }
            set { lock (ShowableSimpleProductLock) { _productsList = value; } }
        }
        public Products()
        {
            InitializeComponent();
            Loaded += Products_Loaded;
            DataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
        }

        private void Products_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBarStart();

            Counter.Event += (s, q) =>
            {
                ProgressBarStop();
                AddNew.IsEnabled = true;
            };
            RefreshData();
            GetAll();
        }
        void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null && dg.SelectedIndex >= 0)
            {
                DataGridRow dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                if (e.Key == Key.Delete && !dgr.IsEditing)
                {
                    // User is attempting to delete the row
                    var resul = MessageBoxesHelper.ShowYesNoMessage("Delete", "About to delete the current row.\n\nProceed?");
                    if (resul == MessageDialogResult.Negative)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        var cat = (ShowableSimpleProduct)dgr.Item;
                        RemoveProduct(cat);
                    }
                }
            }
        }
        private void RemoveProduct(ShowableSimpleProduct supplier)
        {
            RestClient.Client().RemoveProduct(supplier.Id,
                              (response, handle) =>
                              {
                                  if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                                  {
                                      MessageBoxesHelper.ShowWindowInformationAsync("Problem with writing to database",
                                          response.Content);
                                  }
                                  else
                                  {
                                      RefreshData();
                                  }
                              });
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
            Counter.Counter = 1;
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private async void DoRefreshData()
        {
            var tmp = await RestClient.Client().GetProducts();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                ProductLists.Clear();
                foreach (var showableTax in tmp.Data)
                {
                    ProductLists.Add(showableTax);
                }
                DataGrid.Items.Refresh();
            }
            Counter.Counter -= 1;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Counter.Counter > 0)
                return;
            var dg = sender as DataGrid;
            if (dg != null && dg.SelectedIndex >= 0)
            {
                var dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                var product = (ShowableSimpleProduct)dgr.Item;
                if (product.IsSold)
                {

                    var window = new SoldProductDataWindow(product.Id);
                    window.Closed += (s, x) => RefreshData();
                    window.ShowDialog();
                }
                else
                {
                    var window = new StoredProductWindow(product.Id, TaxesNames, CategoriesNames, UnitNames);
                    window.Closed += (s, x) => RefreshData();
                    window.ShowDialog();
                }
            }
        }
        #region Windows helpers

        private void GetAll()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                GetSources();
                GetCategories();
                GetUnits();
                GetRecipies();
            }));
        }

        private void GetUnits()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetUnits));
        }
        private void GetSources()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetTaxes));
        }
        private void GetCategories()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetCategories));
        }

        private void GetRecipies()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetRecipies));

        }
        private async void DoGetUnits()
        {
            var tmp = await RestClient.Client().GetUnits();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                UnitNames = tmp.Data.Select(x => x.Name).ToList();
                Debug.WriteLine("units done");
                Counter.Counter -= 1;
            }
        }
        private async void DoGetRecipies()
        {
            var tmp = await RestClient.Client().GetReceipts();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                RecipiesNames = tmp.Data.Select(x => x.Description).ToList();
                RecipiesNames.Add("");
                Counter.Counter -= 1;
            }
        }
        private async void DoGetCategories()
        {
            var tmp = await RestClient.Client().GetCategories();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                CategoriesNames = tmp.Data.Select(x => x.Name).ToList();
                Counter.Counter -= 1;
            }
        }
        private async void DoGetTaxes()
        {
            var tmp = await RestClient.Client().GetTaxes();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                TaxesNames = tmp.Data.Select(x => x.TaxName).ToList();
                Counter.Counter -= 1;
            }
        }

        #endregion

        private void AddNew_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new ProductAddWindow(TaxesNames, CategoriesNames, UnitNames, RecipiesNames);
            window.Closed +=
                (s, q) => { this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(RefreshData)); };
            window.ShowDialog();
        }
    }
}
