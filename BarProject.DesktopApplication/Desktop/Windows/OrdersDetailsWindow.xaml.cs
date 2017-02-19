using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DesktopApplication.Common.Utils;
using MahApps.Metro.Controls;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    public partial class OrdersDetailsWindow : MetroWindow
    {

        private int _orderId { get; set; }
        private readonly object idLocker = new object();

        public int OrderId
        {
            get
            {
                lock (idLocker)
                {
                    return _orderId;
                }
            }
        }

        private ObservableCollection<ShowableWarehouseOrderDetails> _detailsList;
        private readonly object ShowableSimpleProductLock = new object();

        public ObservableCollection<ShowableWarehouseOrderDetails> DetailsList
        {
            get
            {
                lock (ShowableSimpleProductLock)
                {
                    if (_detailsList == null)
                        _detailsList =
                            new ObservableCollection<ShowableWarehouseOrderDetails>();
                    return _detailsList;
                }
            }
            set { lock (ShowableSimpleProductLock) { _detailsList = value; } }
        }
        public OrdersDetailsWindow(int id, List<string> Products)
        {
            _orderId = id;
            InitializeComponent();
            Loaded += OrdersDetailsWindow_Loaded;
            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
            DataGridCombo.ItemsSource = Products;
        }

        private bool DetailIsEmpty(ShowableWarehouseOrderDetails detail)
        {
            return false;
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {
                var detail = (ShowableWarehouseOrderDetails)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                ProgressBarStart();
                string message = "";
                if (DetailIsEmpty(detail))
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    ProgressBarStop();
                    return;
                }
                if (string.IsNullOrEmpty(detail.Name))
                {
                    message = "You cannot create category with empty name";
                }
                if (detail.Id == null && DetailsList.Count(x => x.Name == detail.Name) > 1)
                {
                    message = "Products cannot be duplicated";
                }
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
                if (detail.Id == null) // new row added
                {
                    AddDetail(detail);
                }
                else
                {
                    UpdateDetail(detail);
                }
            }
        }
        private void AddDetail(ShowableWarehouseOrderDetails detail)
        {

            RestClient.Client().AddWarehouseOrderDetails(OrderId, detail, (response, handle) =>
             {
                 if (response.ResponseStatus != ResponseStatus.Completed ||
                     response.StatusCode != HttpStatusCode.OK)
                 {
                     MessageBoxesHelper.ShowProblemWithRequest(response);
                 }
                 RefreshData();
             });
        }
        private void UpdateDetail(ShowableWarehouseOrderDetails detail)
        {
            RestClient.Client().UpdateWarehouseOrderDetails(OrderId, detail, (response, handle) =>
             {
                 if (response.ResponseStatus != ResponseStatus.Completed ||
                     response.StatusCode != HttpStatusCode.OK)
                 {
                     MessageBoxesHelper.ShowProblemWithRequest(response);
                 }
                 RefreshData(); 
             });
        }


        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void OrdersDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }
        private void RefreshData()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private async void DoRefreshData()
        {
            var tmp = await RestClient.Client().GetWarehouseOrdersDetails(OrderId);
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                DetailsList.Clear();
                foreach (var showableTax in tmp.Data)
                {
                    DetailsList.Add(showableTax);
                }
                DataGrid.Items.Refresh();
                ProgressBarStop();
            }
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
