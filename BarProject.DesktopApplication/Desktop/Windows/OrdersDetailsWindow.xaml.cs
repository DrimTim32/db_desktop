using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DesktopApplication.Common.Utils;
using BarProject.DesktopApplication.Desktop.Controls.Menagement;
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
        public OrdersDetailsWindow(int id, List<string> Products, List<string> Categories)
        {
            _orderId = id;
            InitializeComponent();
            Loaded += OrdersDetailsWindow_Loaded;
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
