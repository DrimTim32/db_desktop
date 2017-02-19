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
                Header = "Orders",
                Content = new Orders()
            };
            orders.SetResourceReference(StyleProperty, "MenuLevel2");
            TabControl.Items.Clear();
            TabControl.Items.Add(orders);
        }
    }
}
