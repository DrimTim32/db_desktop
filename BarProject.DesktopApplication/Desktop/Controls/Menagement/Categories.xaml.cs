using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace BarProject.DesktopApplication.Desktop.Controls.Menagement
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Net;
    using System.Threading;
    using System.Windows.Threading;
    using Common.Utils;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ReadModels;
    using MahApps.Metro.Controls.Dialogs;
    using RestSharp;
    using RestClient = Library.RestHelpers.RestClient;

    /// <summary>
    /// Interaction logic for Categories.xaml
    /// </summary>
    public partial class Categories : UserControl
    {
        private ObservableCollection<ShowableCategory> _categories;
        private object CategoriesLock = new object();
        private object PossibleOverridingCategoriesLock = new object();
        public ObservableCollection<ShowableCategory> CategoriesList
        {
            get
            {
                lock (CategoriesLock)
                {
                    if (_categories == null)
                        _categories = new ObservableCollection<ShowableCategory>();
                    return _categories;
                }
            }
            set
            {

                lock (CategoriesLock)
                {
                    _categories = value;
                }
            }
        }
        private List<string> PossibleOverridingCategories
        {
            get
            {
                lock (PossibleOverridingCategoriesLock)
                {
                    var list = CategoriesList.Select(x => x.Name).ToList();
                    list.Add("");
                    return list;
                }
            }
        }
        public Categories()
        {
            InitializeComponent();
            Loaded += Categories_Loaded;
            DataGridCombo.ItemsSource = PossibleOverridingCategories;

            DataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
            DataGrid.RowEditEnding += DataGrid_RowEditEnding;
        }

        private void Categories_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }

        private bool CategoryIsEmpty(ShowableCategory cat)
        {
            return cat.Name == null && cat.Slug == null;
        }
        private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var grid = sender as DataGrid;
            if (DataGrid.SelectedItem != null && grid != null)
            {
                var cat = (ShowableCategory)e.Row.Item;
                grid.RowEditEnding -= DataGrid_RowEditEnding;
                grid.CommitEdit();
                ProgressBarStart();
                string message = "";
                if (CategoryIsEmpty(cat))
                {
                    grid.CancelEdit();
                    grid.RowEditEnding += DataGrid_RowEditEnding;
                    return;
                }
                if (string.IsNullOrEmpty(cat.Name))
                {
                    message = "You cannot create category with empty name";
                }
                if (string.IsNullOrEmpty(cat.Slug))
                {
                    message = "You cannot create category with empty slug";
                }
                if (cat.Overriding == cat.Name)
                {
                    message = "Jakis ladny ze tak nie mozna";
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
                    AddCategory(cat);
                }
                else
                {
                    UpdateCategory(cat);
                }
            }
        }

        private void AddCategory(ShowableCategory cat)
        {

            RestClient.Client().AddCategory(cat, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                RefreshData();
            });
        }
        private void UpdateCategory(ShowableCategory cat)
        {
            RestClient.Client().UpdateCategory(cat, (response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed ||
                    response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                RefreshData();
            });
        }

        private void RemoveCategory(ShowableCategory cat)
        {
            RestClient.Client().RemoveCategory(cat.Id,
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
                        var cat = (ShowableCategory)dgr.Item;
                        RemoveCategory(cat);
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
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }

        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetCategories();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                CategoriesList.Clear();
                foreach (var showableCategory in tmp.Data)
                {
                    CategoriesList.Add(showableCategory);
                }
                RefreshOverridingPossibilities();
            }
            ProgressBarStop();
        }
        private void RefreshOverridingPossibilities()
        {
            DataGridCombo.ItemsSource = PossibleOverridingCategories;
        }

    }
}
