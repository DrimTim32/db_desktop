using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarProject.DesktopApplication.Remote
{
    /// <summary>
    /// Interaction logic for Subpages.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            var window = new OrderCodeWindow();
            window.ShowDialog();
            var name = window.OrderId;
            var wind = Window.GetWindow(this) as MainRemoteWindow;
            wind.CurrentOrder.Name = name ?? RandomString(10);
            var page = new CategoriesPage();
            NavigationService.Navigate(page);

        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void Orders_Show(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Orders());
        }
    }
}
