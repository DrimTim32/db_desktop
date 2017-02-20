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
using BarProject.DatabaseProxy.Functions;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.WriteModels;
using BarProject.DesktopApplication.Common.Utils;
using BarProject.DesktopApplication.Desktop.Controls.Menagement;
using BarProject.DesktopApplication.Desktop.Windows;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Controls.Warehouse
{
    /// <summary>
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Orders : UserControl
    {
        private List<string> _productNames;
        private List<string> _categoriesNames;



        private readonly SafeCounter _counter = new SafeCounter();
        private readonly object counterLocker = new object();
        private readonly object productsLocker = new object();
        private readonly object categoriesLocker = new object();
        public List<string> ProductNames
        {
            get { lock (productsLocker) { return _productNames; } }
            set { lock (productsLocker) { _productNames = value; } }
        }
        public List<string> CategoriesNames
        {
            get { lock (categoriesLocker) { return _categoriesNames; } }
            set { lock (categoriesLocker) { _categoriesNames = value; } }
        }
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
        private ObservableCollection<ShowableWarehouseOrder> _ordersList;
        private readonly object OrdersLock = new object();

        public ObservableCollection<ShowableWarehouseOrder> OrdersList
        {
            get
            {
                lock (OrdersLock)
                {
                    if (_ordersList == null)
                        _ordersList =
                            new ObservableCollection<ShowableWarehouseOrder>();
                    return _ordersList;
                }
            }
            set { lock (OrdersLock) { _ordersList = value; } }
        }
        public Orders()
        {
            InitializeComponent();
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
            DataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
            Loaded += Orders_Loaded;
        }


        private void Orders_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBarStart();
            Counter.Counter = 4;
            DataGrid.IsReadOnly = true;
            Counter.Event += (s, q) =>
            {
                ProgressBarStop();
                DataGrid.IsReadOnly = false;

            };
            RefreshData();
            GetAll();
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
            var tmp = await RestClient.Client().GetWarehouseOrders();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                OrdersList.Clear();
                foreach (var location in tmp.Data)
                {
                    OrdersList.Add(location);
                }
                DataGrid.Items.Refresh();
                ProgressBarStop();
            }
        }

        private bool IsOrderEmpty(ShowableWarehouseOrder order)
        {
            return false;
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {

                var order = (ShowableWarehouseOrder)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                ProgressBarStart();
                if (IsOrderEmpty(order))
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    return;
                }
                var message = "";
                if (string.IsNullOrEmpty(order.LocationName))
                    message = "You must provide location";
                if (string.IsNullOrEmpty(order.SupplierName))
                    message = "You must provide supplier";
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
                    UpdateOrder(order);
                else
                    AddOrder(order);
            }
        }

        private void UpdateOrder(ShowableWarehouseOrder order)
        {

            RestClient.Client().UpdateWarehouseOrder(order, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                RefreshData();
            });
        }
        private void AddOrder(ShowableWarehouseOrder order)
        {
            RestClient.Client().AddWarehouseOrder(order, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                RefreshData();
            });
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Counter.Counter != 0)
                return;
            var dg = sender as DataGrid;
            if (dg != null && dg.SelectedIndex >= 0 && dg.SelectedIndex < dg.Items.Count - 1)
            {
                var dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                var product = dgr.Item as ShowableWarehouseOrder;
                if (product?.Id == null)
                    return;

                var window = new OrdersDetailsWindow(product.Id.Value, ProductNames);
                window.Closed += (s, x) => RefreshData();
                window.ShowDialog();
            }
        }
        #region Windows helpers

        private void GetAll()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                GetProductNames();
                GetCategoriesNames();
                GetSuppliersNames();
                GetLocationsNames();
            }));
        }

        private void GetProductNames()
        {
            ProgressBarStart();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetProducts));
        }
        private void GetSuppliersNames()
        {
            ProgressBarStart();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetSuppliers));
        }
        private void GetLocationsNames()
        {
            ProgressBarStart();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetLocations));
        }

        private void GetCategoriesNames()
        {
            ProgressBarStart();
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetCategoriesNames));
        }
        private async void DoGetSuppliers()
        {
            var tmp = await RestClient.Client().GetSuppliers();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                SupplierBox.ItemsSource = tmp.Data.Select(x => x.Name).ToList();
                Counter.Counter -= 1;
            }
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
                LocationBox.ItemsSource = tmp.Data.Select(x => x.Name).ToList();
                Counter.Counter -= 1;
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
                Counter.Counter -= 1;
            }
        }
        private async void DoGetCategoriesNames()
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
        #endregion

        private void MarkAsDelivered_Click(object sender, RoutedEventArgs e)
        {

            var dg = DataGrid;
            if (dg != null && dg.SelectedIndex >= 0)
            {
                var dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                var product = dgr.Item as ShowableWarehouseOrder;
                if (product?.Id == null)
                    return;
                RestClient.Client().MarkAsDelivered(product.Id.Value, (response, handle) =>
                {
                    if (response.ResponseStatus != ResponseStatus.Completed ||
                        response.StatusCode != HttpStatusCode.OK)
                    {
                        MessageBoxesHelper.ShowProblemWithRequest(response);
                    }
                    RefreshData();
                });
            }
        }
    }
}
