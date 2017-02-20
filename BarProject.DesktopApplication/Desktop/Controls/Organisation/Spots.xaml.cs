using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using MahApps.Metro.Controls.Dialogs;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Controls.Organisation
{
    /// <summary>
    /// Interaction logic for Spots.xaml
    /// </summary>
    public partial class Spots : UserControl
    {
        public List<string> LocationNames { get; set; }
        private ObservableCollection<ShowableSpot> _spotList;
        private readonly object SpostLock = new object();

        public ObservableCollection<ShowableSpot> SpotsList
        {
            get
            {
                lock (SpostLock)
                {
                    if (_spotList == null)
                        _spotList =
                            new ObservableCollection<ShowableSpot>();
                    return _spotList;
                }
            }
            set
            {
                lock (SpostLock)
                {
                    _spotList = value;
                }
            }
        }
        private void GetAll()
        {
            Dispatcher.BeginInvoke(new Action(GetLocations));
        }

        private void GetLocations()
        {
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
                DataGridCombo.ItemsSource = tmp.Data.Select(x => x.Name).ToList();
            }
        }
        public Spots()
        {
            InitializeComponent();
            Loaded += Locations_Loaded;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
            GetAll();
        }

        private void Locations_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => Progress.Visibility = Visibility.Hidden));
        }

        private void RefreshData()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }

        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetSpots();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                SpotsList.Clear();
                foreach (var location in tmp.Data)
                {
                    SpotsList.Add(location);
                }
                DataGrid.Items.Refresh();
                ProgressBarStop();
            }
        }

        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {

                var location = (ShowableSpot)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                ProgressBarStart();
                if (string.IsNullOrEmpty(location.Name))
                {
                    var message = "You cannot create spot with empty name";
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    MessageBoxesHelper.ShowWindowInformationAsync("Problem with new item!", message);
                    grid.Items.Refresh();
                    RefreshData();
                    ProgressBarStop();
                    return;
                }
                grid.RowEditEnding += DataGrid_RowEditEnding;
                if (location.Id == null)
                {
                    RestClient.Client().AddSpot(location, (response, handle) =>
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
                    RestClient.Client().UpdateSpot(location, (response, handle) =>
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

        void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null && dg.SelectedIndex >= 0 && dg.SelectedIndex < dg.Items.Count - 1)
            {
                DataGridRow dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                if (e.Key == Key.Delete && !dgr.IsEditing)
                {
                    // User is attempting to delete the row
                    var resul = MessageBoxesHelper.ShowYesNoMessage("Delete",
                        "About to delete the current row.\n\nProceed?");
                    if (resul == MessageDialogResult.Negative)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        var cat = (ShowableSpot)dgr.Item;
                        RestClient.Client().RemoveSpot(cat.Id,
                            (response, handle) =>
                            {
                                if (response.ResponseStatus != ResponseStatus.Completed ||
                                    response.StatusCode != HttpStatusCode.OK)
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