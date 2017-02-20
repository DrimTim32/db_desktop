using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.WriteModels;
using BarProject.DesktopApplication.Common.Utils;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Remote
{
    using MahApps.Metro.Controls;
    public partial class MainRemoteWindow : MetroWindow
    {
        public static List<WritableOrder> OrderToDo { get; set; } = new List<WritableOrder>();
        public WritableOrder CurrentOrder { get; set; } = new WritableOrder();
        public MainRemoteWindow(string username)
        {
            InitializeComponent();
            this.InfoLabel.Content = $"Logged as {username}";
        }

        public void RegisterProduct(ShowableSoldProduct product, short quantity)
        {
            CurrentOrder.AddProduct(product, quantity);
        }
        public void AcceptOrder()
        {
            if (CurrentOrder.Sum == 0)
            {
                CurrentOrder.Clear();
                OrderDetails.Visibility = Visibility.Hidden;
                return;
            }
            RestClient.Client().AddUserOrder(CurrentOrder, ((response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        CurrentOrder.Id = response.Data;
                        QueueOrder(CurrentOrder);
                        CreateNewOrder();
                        OrderDetails.Visibility = Visibility.Hidden;
                    }));
                }
            }));

        }

        private void QueueOrder(WritableOrder order)
        {
            OrderToDo.Add(new WritableOrder(order));
        }
        public void CreateNewOrder()
        {
            OrderDetails.Visibility = Visibility.Visible;
            CurrentOrder.Clear();
        }
        public void DiscardOrder()
        {
            CreateNewOrder();
            OrderDetails.Visibility = Visibility.Hidden;
        }

        public void GoToFirstPage()
        {
            while (Frame.NavigationService.CanGoBack)
            {
                Frame.NavigationService.GoBack();
            }
        }
        private void CloseOrder_Click(object sender, RoutedEventArgs e)
        {
            AcceptOrder();
            GoToFirstPage();
        }
        private void DiscardOrder_Click(object sender, RoutedEventArgs e)
        {
            DiscardOrder();
            GoToFirstPage();
        }

        public void SetAsPaid(WritableOrder order)
        {
            RestClient.Client().MarkPaid(order.Id, ((response, handle) =>
            {
                if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowProblemWithRequest(response);
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        OrderToDo.Remove(order);
                        OrderDetails.Visibility = Visibility.Hidden;
                    }));
                }
            }));
        }
    }
}
