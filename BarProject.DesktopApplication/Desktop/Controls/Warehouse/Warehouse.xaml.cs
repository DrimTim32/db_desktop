using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DesktopApplication.Common.Utils;
using BarProject.DesktopApplication.Desktop.Windows;
using MahApps.Metro.Controls.Dialogs;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Controls.Warehouse
{
    /// <summary>
    /// Interaction logic for Warehouse.xaml
    /// </summary>
    public partial class Warehouse : UserControl
    {
        private List<string> _productNames;
        private List<string> _locationNames;
        private readonly object productsLocker = new object();
        private readonly object locationsLocker = new object();
        public List<string> ProductNames
        {
            get { lock (productsLocker) { return _productNames; } }
            set { lock (productsLocker) { _productNames = value; } }
        }
        public List<string> LocationNames
        {
            get { lock (locationsLocker) { return _locationNames; } }
            set { lock (locationsLocker) { _locationNames = value; } }
        }
        private ObservableCollection<ShowableWarehouseItem> _categories;
        private object PossibleOverridingCategoriesLock = new object();
        public ObservableCollection<ShowableWarehouseItem> ItemsList
        {
            get
            {
                lock (PossibleOverridingCategoriesLock)
                {
                    if (_categories == null)
                        _categories = new ObservableCollection<ShowableWarehouseItem>();
                    return _categories;
                }
            }
            set
            {

                lock (PossibleOverridingCategoriesLock)
                {
                    _categories = value;
                }
            }
        }
        public Warehouse()
        {
            InitializeComponent();
            Loaded += Warehouse_Loaded;
            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
        }

        private void Warehouse_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
            GetAll();
        }
        private void RefreshData()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetWarehouse();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                ItemsList.Clear();
                foreach (var location in tmp.Data)
                {
                    ItemsList.Add(location);
                }
                DataGrid.Items.Refresh();
                ProgressBarStop();
            }
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
                        var cat = (ShowableWarehouseItem)dgr.Item;
                        RemoveItem(cat);
                    }
                }
            }
        }
        private void RemoveItem(ShowableWarehouseItem order)
        {

            RestClient.Client().RemoveWarehouse(order, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                RefreshData();
            });
        }
        private bool IsItemEmpty(ShowableWarehouseItem item)
        {
            return item.LocationName == null && item.ProductName == null;
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {

                var order = (ShowableWarehouseItem)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                ProgressBarStart();
                if (IsItemEmpty(order))
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    return;
                }
                var message = "";
                if (string.IsNullOrEmpty(order.LocationName))
                    message = "You must provide location";
                if (string.IsNullOrEmpty(order.ProductName))
                    message = "You must provide product";
                if (message != "")
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    MessageBoxesHelper.ShowWindowInformationAsync("Problem with new item!", message);
                    RefreshData();
                    ProgressBarStop();
                    return;
                }
                grid.RowEditEnding += DataGrid_RowEditEnding;
                if (order.Id != null)
                    UpdateItem(order);
                else
                    AddItem(order);
            }
        }

        private void UpdateItem(ShowableWarehouseItem order)
        {

            RestClient.Client().UpdateWarehouse(order, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.Content.Contains("unique"))
                    {

                        MessageBoxesHelper.ShowWindowInformationAsync("Error", "Pair product,location must be unique");
                    }
                    else
                        MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                RefreshData();
            });
        }
        private void AddItem(ShowableWarehouseItem order)
        {
            RestClient.Client().AddWarehouse(order, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.Content.Contains("UNIQUE"))
                    {

                        MessageBoxesHelper.ShowWindowInformationAsync("Error", "Pair product,location must be unique");
                    }
                    else
                        MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                RefreshData();
            });
        }


        #region helpers
        private void GetAll()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                GetProductNames();
                GetLocationsNames();
            }));
        }

        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        private void GetProductNames()
        {
            ProgressBarStart();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetProducts));
        }
        private void GetLocationsNames()
        {
            ProgressBarStart();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetLocations));
        }

        private async void DoGetLocations()
        {
            var tmp = await RestClient.Client().GetLocations();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                LocationNames = tmp.Data.Select(x => x.Name).ToList();
                LocationsColumn.ItemsSource = LocationNames;
            }
        }
        private async void DoGetProducts()
        {
            var tmp = await RestClient.Client().GetOrderableProductNames();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                ProductNames = tmp.Data.ToList();
                ProductsColumn.ItemsSource = ProductNames;
            }
        }
        #endregion
    }
}
