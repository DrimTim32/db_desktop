using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DesktopApplication.Common.Utils;
using BarProject.DesktopApplication.Desktop.Windows;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Controls.Menagement
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for Recipies.xaml
    /// </summary>
    public partial class Recipies : UserControl
    {
        private ObservableCollection<ShowableReceipt> _recipiesData;
        private readonly object productsLocker = new object();
        private List<string> _productNames;
        private readonly object ReceiptDataLock = new object();
        public List<string> ProductNames
        {
            get { lock (productsLocker) { return _productNames; } }
            set { lock (productsLocker) { _productNames = value; } }
        }
        public ObservableCollection<ShowableReceipt> RecipiesData
        {
            get
            {

                lock (ReceiptDataLock)
                {
                    if (_recipiesData == null)
                        _recipiesData = new ObservableCollection<ShowableReceipt>();
                    return _recipiesData;
                }
            }
            set
            {
                lock (ReceiptDataLock)
                {
                    _recipiesData = value;
                }

            }
        }
        public Recipies()
        {
            InitializeComponent();
            Loaded += Recipies_Loaded;
            DataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {
                var cat = (ShowableReceipt)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                ProgressBarStart();
                string message = "";
                if (cat.Description == null)
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    return;
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
                if (cat.Id == null) // new row added
                {
                    AddRecipe(cat);
                }
                else
                {
                    UpdateRecipe(cat);
                }
            }
        }

        private void AddRecipe(ShowableReceipt recipit)
        {
            RestClient.Client().AddReceipt(recipit, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                RefreshData();
            });
        }
        private void UpdateRecipe(ShowableReceipt recipit)
        {
            RestClient.Client().UpdateReceipt(recipit, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                RefreshData();
            });
        }
        private void Recipies_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RefreshData();
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetReceipts();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                RecipiesData.Clear();
                foreach (var showableTax in tmp.Data)
                {
                    RecipiesData.Add(showableTax);
                }
                DataGrid.Items.Refresh();
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
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetProducts));
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
            }
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg != null && dg.SelectedIndex >= 0 && dg.SelectedIndex < dg.Items.Count - 1)
            {
                var dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                var receipt = (ShowableReceipt)dgr.Item;
                if (receipt.Id == null)
                    return;
                var window = new RecipieDetailsWindow(receipt.Id.Value, ProductNames);
                window.ShowDialog();
            }
        }

    }
}
