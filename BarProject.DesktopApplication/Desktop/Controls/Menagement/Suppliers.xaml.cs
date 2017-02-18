namespace BarProject.DesktopApplication.Desktop.Controls.Menagement
{
    using System;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Common.Utils;
    using DatabaseProxy.Models.ReadModels;
    using MahApps.Metro.Controls.Dialogs;
    using RestSharp;
    using RestClient = Library.RestHelpers.RestClient;

    /// <summary>
    /// Interaction logic for Suppliers.xaml
    /// </summary>
    public partial class Suppliers : UserControl
    {
        private ObservableCollection<ShowableSupplier> _suppliersList;
        private readonly object SuppliesLock = new object();

        public ObservableCollection<ShowableSupplier> SuppliersList
        {
            get
            {
                lock (SuppliesLock)
                {
                    if (_suppliersList == null)
                        _suppliersList =
                            new ObservableCollection<ShowableSupplier>();
                    return _suppliersList;
                }
            }
            set { lock (SuppliesLock) { _suppliersList = value; } }
        }
        public Suppliers()
        {
            InitializeComponent();
            Loaded += Suppliers_Loaded;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
        }
        private void Suppliers_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
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
        private bool IsSupplierEmpty(ShowableSupplier location)
        {
            return location.Name == null && location.City == null && location.Country == null && location.City == null &&
                   location.Phone == null && location.Address == null && location.PostalCode == null;
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetSuppliers();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                SuppliersList.Clear();
                foreach (var location in tmp.Data)
                {
                    SuppliersList.Add(location);
                }
                DataGrid.Items.Refresh();
            }
            ProgressBarStop();
        }
        void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null)
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
                        var cat = (ShowableSupplier)dgr.Item;
                        RemoveSupplier(cat);
                    }
                }
            }
        }

        private void RemoveSupplier(ShowableSupplier supplier)
        {
            RestClient.Client().RemoveSupplier(supplier.Id,
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

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {

                var supplier = (ShowableSupplier)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                ProgressBarStart();
                if (IsSupplierEmpty(supplier))
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    return;
                }
                if (string.IsNullOrEmpty(supplier.Name))
                {
                    var message = "You cannot create location with empty name";
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    MessageBoxesHelper.ShowWindowInformationAsync("Problem with new item!", message);
                    grid.Items.Refresh();
                    RefreshData();
                    ProgressBarStop();
                    return;
                }
                grid.RowEditEnding += DataGrid_RowEditEnding;
                if (supplier.Id != null)
                    UpdateSupplier(supplier);
                else
                    AddSupplier(supplier);
            }
        }
        private void AddSupplier(ShowableSupplier supplier)
        {
            RestClient.Client().AddSupplier(supplier, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowWindowInformationAsync("Problem with writing to database",
                        response.Content.Replace("Reason", ""));
                }
                RefreshData();
            });
        }
        private void UpdateSupplier(ShowableSupplier supplier)
        {
            RestClient.Client().UpdateSupplier(supplier, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowWindowInformationAsync("Problem with writing to database",
                        response.Content.Replace("Reason", ""));
                }
                RefreshData();
            });

        }

    }
}
