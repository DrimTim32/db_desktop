using System;
using System.Collections.ObjectModel;
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
        private object ReceiptDataLock = new object();

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
            this.DataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
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
        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg != null && dg.SelectedIndex >= 0)
            {
                var dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                var receipt = (ShowableReceipt)dgr.Item;

                var window = new RecipieDetailsWindow(receipt.Id.Value);
                window.ShowDialog();
            }
        }

    }
}
