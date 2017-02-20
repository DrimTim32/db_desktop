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
using MahApps.Metro.Controls;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    /// <summary>
    /// Interaction logic for ClientOrderDetailsWindow.xaml
    /// </summary>
    public partial class ClientOrderDetailsWindow : MetroWindow
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
        private ObservableCollection<ShowableClientOrderDetails> _detailsList;
        private readonly object ShowableSimpleProductLock = new object();

        public ObservableCollection<ShowableClientOrderDetails> DetailsList
        {
            get
            {
                lock (ShowableSimpleProductLock)
                {
                    if (_detailsList == null)
                        _detailsList =
                            new ObservableCollection<ShowableClientOrderDetails>();
                    return _detailsList;
                }
            }
            set { lock (ShowableSimpleProductLock) { _detailsList = value; } }
        }
        public ClientOrderDetailsWindow(int id)
        {
            _orderId = id;
            InitializeComponent();
            Loaded += ClientOrderDetailsWindow_Loaded; ;

        }

        private void ClientOrderDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }
        private void RefreshData()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetClientOrdersDetails(OrderId);
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
                ProgressBarStop();
            }
        }
    }
}
