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
using MahApps.Metro.Controls.Dialogs;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    public partial class RecipieDetailsWindow : MetroWindow
    {
        private ObservableCollection<ShowableRecipitDetail> _detailsList;
        private readonly object DetailsLock = new object();
        private readonly object IdLock = new object();
        private int _id;
        public ObservableCollection<ShowableRecipitDetail> DetailsList
        {
            get
            {
                lock (DetailsLock)
                {
                    if (_detailsList == null)
                        _detailsList =
                            new ObservableCollection<ShowableRecipitDetail>();
                    return _detailsList;
                }
            }
            set { lock (DetailsLock) { _detailsList = value; } }
        }

        private int Id
        {
            get { lock (IdLock) { return _id; } }
            set
            {
                lock (IdLock)
                {
                    _id = value;
                }
            }
        }

        public RecipieDetailsWindow(int id, List<string> productNames)
        {
            Id = id;
            InitializeComponent();
            DataGridCombo.ItemsSource = productNames;
            Loaded += RecipieDetailsWindow_Loaded;

            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
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
                    var resul = MessageBoxesHelper.ShowYesNoMessage("Delete", "About to delete the current row.\n\nProceed?");
                    if (resul == MessageDialogResult.Negative)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        var cat = (ShowableRecipitDetail)dgr.Item;
                        RemoveDetails(cat);
                    }
                }
            }
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {
                var cat = (ShowableRecipitDetail)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                ProgressBarStart();
                string message = "";
                if (string.IsNullOrEmpty(cat.ProductName))
                    message = "Product name must not be empty";
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
                if (cat.ProductId == null) // new row added
                {
                    if (DetailsList.Count(x => x.ProductName == cat.ProductName) > 1)
                    {
                        MessageBoxesHelper.ShowWindowInformationAsync("Problem with new item!", "You cannot add the same product once more");
                        RefreshData();
                        ProgressBarStop();
                    }
                    else
                        AddRecipieDetails(cat);
                }
                else
                {
                    UpdateRecipieDetails(cat);
                }
            }
        }
        private void AddRecipieDetails(ShowableRecipitDetail cat)
        {

            RestClient.Client().AddReceiptDetails(Id, cat, (response, handle) =>
             {
                 if (response.ResponseStatus != ResponseStatus.Completed ||
                     response.StatusCode != HttpStatusCode.OK)
                 {
                     MessageBoxesHelper.ShowProblemWithRequest(response);
                 }
                 RefreshData();
             });
        }
        private void UpdateRecipieDetails(ShowableRecipitDetail cat)
        {
            RestClient.Client().UpdateReceiptDetails(Id, cat, (response, handle) =>
             {
                 if (response.ResponseStatus != ResponseStatus.Completed ||
                     response.StatusCode != HttpStatusCode.OK)
                 {
                     MessageBoxesHelper.ShowProblemWithRequest(response);
                 }
                 RefreshData();
             });
        }
        private void RemoveDetails(ShowableRecipitDetail cat)
        {
            RestClient.Client().RemoveReceiptDetails(Id, cat.ProductId,
                (response, handle) =>
                {
                    if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                    {
                        MessageBoxesHelper.ShowProblemWithRequest(response);
                    }
                    else
                    {
                        RefreshData();
                    }
                });
        }
        private void RecipieDetailsWindow_Loaded(object sender, RoutedEventArgs e)
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
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private void DoRefreshData()
        {
            ProgressBarStart();
            RestClient.Client().GetReceiptDetails(Id, (response) =>
                {
                    if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                    {
                        MessageBoxesHelper.ShowProblemWithRequest(response);
                    }
                    else
                    {
                        Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        {
                            DetailsList.Clear();
                            foreach (var showableCategory in response.Data)
                            {
                                DetailsList.Add(showableCategory);
                            }
                        }));
                    }
                    ProgressBarStop();
                }
            );
        }


    }
}
