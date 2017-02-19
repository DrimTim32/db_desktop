using System.Windows;
using System.Windows.Navigation;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.WriteModels;
using BarProject.DesktopApplication.Library.RestHelpers;

namespace BarProject.DesktopApplication.Remote
{
    using MahApps.Metro.Controls;
    public partial class MainRemoteWindow : MetroWindow
    {
        public WritableOrder Order { get; set; } = new WritableOrder();
        public MainRemoteWindow(string username)
        {
            InitializeComponent();
            this.InfoLabel.Content = $"Logged as {username}";
        }

        public void RegisterProduct(ShowableSoldProduct product, short quantity)
        {
            OrderDetails.Visibility = Visibility.Visible;
            Order.AddProduct(product, quantity);
        }
        public void AcceptOrder()
        {
            RestClient.Client().AddOrder(Order, ((response, handle) =>
            {
                Order.Clear();
                OrderDetails.Visibility = Visibility.Hidden;
            }));

        }
        public void DiscardOrder()
        {
            Order.Clear();
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
        }
        private void DiscardOrder_Click(object sender, RoutedEventArgs e)
        {
            DiscardOrder();
            GoToFirstPage();
        }

    }
}
