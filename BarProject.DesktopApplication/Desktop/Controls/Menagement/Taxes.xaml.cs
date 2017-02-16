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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarProject.DesktopApplication.Desktop.Controls.Menagement
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Net;
    using System.Windows.Threading;
    using Common.Utils;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ReadModels;
    using MahApps.Metro.Controls.Dialogs;
    using RestSharp;
    using RestClient = Library.RestHelpers.RestClient;

    /// <summary>
    /// Interaction logic for Taxes.xaml
    /// </summary>
    public partial class Taxes : UserControl
    {
        private ObservableCollection<ShowableTax> _taxesData;
        private object TaxesDataLock = new object();

        public ObservableCollection<ShowableTax> TaxesData
        {
            get
            {

                lock (TaxesDataLock)
                {
                    if (_taxesData == null)
                        _taxesData = new ObservableCollection<ShowableTax>();
                    return _taxesData;
                }
            }
            set
            {
                lock (TaxesDataLock)
                {
                    _taxesData = value;
                }

            }
        }
        public Taxes()
        {
            InitializeComponent();
            Loaded += Taxes_Loaded;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
        }
        private void Taxes_Loaded(object sender, RoutedEventArgs e)
        {

            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private bool IsTaxEmpty(ShowableTax tax)
        {
            return tax.TaxName == null && tax.TaxValue == null;
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {

                var tax = (ShowableTax)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                string message = "";
                ProgressBarStart();
                if (IsTaxEmpty(tax))
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    return;
                }
                if (string.IsNullOrEmpty(tax.TaxName))
                    message = "You cannot create tax with empty name";
                if (tax.TaxValue == null)
                    message = "You cannot create tax with empty value";
                if (message != "")
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    MessageBoxesHelper.ShowWindowInformationAsync("Problem with new item!", message);
                    grid.Items.Refresh();
                    RefreshData();
                    ProgressBarStop();
                    return;
                }
                grid.RowEditEnding += DataGrid_RowEditEnding;
                RestClient.Client().AddTax(tax, (response, handle) =>
                {
                    if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response.Content.Contains("INSERT") && response.Content.Contains("CHECK"))
                            MessageBoxesHelper.ShowWindowInformationAsync("Problem with writing to database", "Tax value must be between 0 and 1");
                        else
                            MessageBoxesHelper.ShowWindowInformationAsync("Problem with writing to database", response.Content.Replace("Reason", ""));
                    }
                    RefreshData();
                });

            }
        }
        async void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
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
                        var cat = (ShowableTax)dgr.Item;
                        RestClient.Client().RemoveTax(cat.Id,
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
                }
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

        private void RefreshData()
        {

            this.Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(
                  DoRefreshData
                  ));
        }

        private async void DoRefreshData()
        {
            try
            {
                ProgressBarStart();
                var tmp = await RestClient.Client().GetTaxes();
                TaxesData.Clear();
                foreach (var showableTax in tmp.Data)
                {
                    TaxesData.Add(showableTax);
                }
                DataGrid.Items.Refresh();
                ProgressBarStop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in do refresh data" + ex.Message);
            }
        }
    }
}
