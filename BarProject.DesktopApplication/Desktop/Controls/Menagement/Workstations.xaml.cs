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
    using System.Net;
    using System.Windows.Threading;
    using Common.Utils;
    using DatabaseProxy.Models.ReadModels;
    using MahApps.Metro.Controls.Dialogs;
    using RestSharp;
    using RestClient = Library.RestHelpers.RestClient;

    /// <summary>
    /// Interaction logic for Workstations.xaml
    /// </summary>
    public partial class Workstations : UserControl
    {
        private ObservableCollection<ShowableWorkstation> _productsList;
        private readonly object LocationsLock = new object();

        public ObservableCollection<ShowableWorkstation> WorksationsList
        {
            get
            {
                lock (LocationsLock)
                {
                    if (_productsList == null)
                        _productsList =
                            new ObservableCollection<ShowableWorkstation>();
                    return _productsList;
                }
            }
            set { lock (LocationsLock) { _productsList = value; } }
        }
        public Workstations()
        {
            InitializeComponent();
            Loaded += ItemsLoaded;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
        }
        private void ItemsLoaded(object sender, RoutedEventArgs e)
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

        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetWorkstations();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                WorksationsList.Clear();
                foreach (var location in tmp.Data)
                {
                    WorksationsList.Add(location);
                }
                DataGrid.Items.Refresh();
            }
            ProgressBarStop();
        }

        private bool IsWorkstationEmpty(ShowableWorkstation workstation)
        {
            return workstation.Name == null;
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {

                var workstation = (ShowableWorkstation)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                ProgressBarStart();
                if (IsWorkstationEmpty(workstation))
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    return;
                }
                if (string.IsNullOrEmpty(workstation.Name))
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
                if (workstation.Id == null)
                {
                    RestClient.Client().AddWorkstation(workstation, (response, handle) =>
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
                else
                {
                    RestClient.Client().UpdateWorkstation(workstation, (response, handle) =>
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
                        var cat = (ShowableWorkstation)dgr.Item;
                        RestClient.Client().RemoveWorkstation(cat.Id,
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
