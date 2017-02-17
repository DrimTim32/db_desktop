using System.Windows;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.WriteModels;

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

        public void RegisterProduct(ShowableSoldProduct product, int quantity)
        {
            OrderDetails.Visibility = Visibility.Visible;
            Order.AddProduct(product, quantity);
        }

        public void AcceptOrder()
        {
            Order.Clear();
            OrderDetails.Visibility = Visibility.Hidden;
        }

        private void CloseOrder_Click(object sender, RoutedEventArgs e)
        { 
            AcceptOrder();
        }
    }
}
