using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DesktopApplication.Common.Utils;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Controls.Menagement
{
    using System.Windows.Controls;

    public partial class Prices : UserControl
    {
        private ObservableCollection<ShowablePrices> _showablePricesList;
        private readonly object ShowablePricesLock = new object();

        public ObservableCollection<ShowablePrices> PricesList
        {
            get
            {
                lock (ShowablePricesLock)
                {
                    if (_showablePricesList == null)
                        _showablePricesList =
                            new ObservableCollection<ShowablePrices>();
                    return _showablePricesList;
                }
            }
            set { lock (ShowablePricesLock) { _showablePricesList = value; } }
        }
        public Prices()
        {
            InitializeComponent();
            this.Loaded += Prices_Loaded;
        }

        private void Prices_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
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
            var tmp = await RestClient.Client().GetPrices();
            if (tmp.ResponseStatus != ResponseStatus.Completed || tmp.StatusCode != HttpStatusCode.OK)
            {
                MessageBoxesHelper.ShowProblemWithRequest(tmp);
            }
            else
            {
                PricesList.Clear();
                foreach (var showableCategory in tmp.Data)
                {
                    PricesList.Add(showableCategory);
                }
                DataGrid.Items.Refresh();
            }
            ProgressBarStop();
        }

    }
}
