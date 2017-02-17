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
        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            var page = new CategoriesPage();
            NavigationService.Navigate(page);

        }
    }
}
