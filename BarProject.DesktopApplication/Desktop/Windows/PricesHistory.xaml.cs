using System;
using System.Net;
using System.Windows;
using System.Windows.Threading;
using BarProject.DesktopApplication.Common.Utils;
using MahApps.Metro.Controls;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    public partial class PricesHistory : MetroWindow
    {
        private int _id;
        public PricesHistory(int id)
        {
            _id = id;
            InitializeComponent();
            this.Loaded += PricesHistory_Loaded;
        } 
        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        private void PricesHistory_Loaded(object sender, RoutedEventArgs e)
        {
            ProgressBarStart();
            RestClient.Client().GetPricesHistory(_id, (response, handle) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                    {
                        MessageBoxesHelper.ShowProblemWithRequest(response, this);
                    }
                    else
                    {
                        if (response.Data.Count == 0)
                        {
                            MessageBoxesHelper.ShowWindowInformation("This product doesn't have prices history", "   ", this);
                            Close();
                        }
                        else
                        {
                            Datagrid.ItemsSource = response.Data;
                        }

                    }
                    ProgressBarStop();
                });
            });
        }
    }
}
