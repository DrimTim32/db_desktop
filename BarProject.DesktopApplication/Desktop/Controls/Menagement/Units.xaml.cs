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
    /// Interaction logic for Units.xaml
    /// </summary>
    public partial class Units : UserControl
    {
        public Units()
        {
            InitializeComponent();
            Loaded += Units_Loaded;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown; 
        }
        private void Units_Loaded(object sender, RoutedEventArgs e)
        {
            GetSources();
            RefreshData();

        }

        private void GetSources()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoGetSources));
        }

        private async void DoGetSources()
        {

            ProgressBarStart();
            var tmp = await RestClient.Client().GetUnitsTypes();
            DataGridCombo.ItemsSource = tmp.Data;
            ProgressBarStop();
        }
        private ObservableCollection<ShowableUnit> _unitsList;
        private readonly object UnitsDataLock = new object();
        public ObservableCollection<ShowableUnit> UnitsLits
        {
            get
            {
                lock (UnitsDataLock)
                {
                    if (_unitsList == null)
                        _unitsList = new ObservableCollection<ShowableUnit>();
                    return _unitsList;
                }
            }
            set
            {
                lock (UnitsDataLock) { _unitsList = value; }
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
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }

        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetUnits();
            UnitsLits.Clear();
            foreach (var unit in tmp.Data)
            {
                UnitsLits.Add(unit);
            }
            DataGrid.Items.Refresh();
            ProgressBarStop();
        }


        private bool IsUnitEmpty(ShowableUnit tax)
        {
            return tax.Name == null && tax.Type == null && tax.Factor == null;
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {

                var tax = (ShowableUnit)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                string message = "";
                ProgressBarStart();
                if (IsUnitEmpty(tax))
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    return;
                }
                if (string.IsNullOrEmpty(tax.Name))
                    message = "You cannot create unit with empty name";
                if (string.IsNullOrEmpty(tax.Type))
                    message = "You cannot create unit with empty type";
                if (tax.Factor == null)
                    message = "You cannot create unit with empty factor";
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
                RestClient.Client().AddUnit(tax, (response, handle) =>
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
                        var cat = (ShowableUnit)dgr.Item;
                        RestClient.Client().RemoveUnit(cat.Id,
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

    }
}
