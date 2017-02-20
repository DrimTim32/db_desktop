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

namespace BarProject.DesktopApplication.Desktop.Controls.Warehouse
{
    using MahApps.Metro.Controls;
    using Menagement;

    /// <summary>
    /// Interaction logic for WarehouseUserControl.xaml
    /// </summary>
    public partial class WarehouseUserControl : UserControl
    {
        public WarehouseUserControl()
        {
            InitializeComponent();
            SetTabs();
        }
        public void SetTabs()
        {

            var orders = new MetroTabItem()
            {
                Header = "Warehouse orders",
                Content = new Orders()
            };
            var clientorders = new MetroTabItem()
            {
                Header = "Client orders",
                Content = new ClientOrders()
            };
            var warehouse = new MetroTabItem()
            {
                Header = "Warehouse",
                Content = new Warehouse()
            }; var productsUsage = new MetroTabItem()
            {
                Header = "Products usage",
                Content = new ProductsUsage()
            };
            orders.SetResourceReference(StyleProperty, "MenuLevel2");
            productsUsage.SetResourceReference(StyleProperty, "MenuLevel2");
            warehouse.SetResourceReference(StyleProperty, "MenuLevel2");
            clientorders.SetResourceReference(StyleProperty, "MenuLevel2");
            TabControl.Items.Clear();
            TabControl.Items.Add(orders);
            TabControl.Items.Add(warehouse);
            TabControl.Items.Add(productsUsage);
            TabControl.Items.Add(clientorders);
        }
    }
}
