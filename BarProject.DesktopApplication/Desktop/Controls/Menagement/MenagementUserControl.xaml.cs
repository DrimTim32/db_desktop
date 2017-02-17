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

namespace BarProject.DesktopApplication.Desktop.Controls.Menagement
{
    using MahApps.Metro.Controls;

    public partial class MenagementUserControl : UserControl
    {
        public MenagementUserControl()
        {
            InitializeComponent();
            SetTabs();
        }

        private void SetTabs()
        {
            var categories = new MetroTabItem()
            {
                Header = "Categories",
                Content = new Categories()
            };

            var taxes = new MetroTabItem()
            {
                Header = "Taxes",
                Content = new Taxes()
            };
            var units = new MetroTabItem()
            {
                Header = "Units",
                Content = new Units()
            };
            var suppliers = new MetroTabItem()
            {
                Header = "Suppliers",
                Content = new Suppliers()
            };
            var prices = new MetroTabItem()
            {
                Header = "Prices",
                Content = new Prices()
            };

            var products = new MetroTabItem()
            {
                Header = "Products",
                Content = new Products()
            };
            var recipies = new MetroTabItem()
            {
                Header = "Recipies",
                Content = new Recipies()
            };
            var workstations = new MetroTabItem()
            {
                Header = "Recipies",
                Content = new Workstations()
            };
            categories.SetResourceReference(StyleProperty, "MenuLevel2");
            taxes.SetResourceReference(StyleProperty, "MenuLevel2");
            units.SetResourceReference(StyleProperty, "MenuLevel2");
            suppliers.SetResourceReference(StyleProperty, "MenuLevel2");
            prices.SetResourceReference(StyleProperty, "MenuLevel2");
            products.SetResourceReference(StyleProperty, "MenuLevel2");
            recipies.SetResourceReference(StyleProperty, "MenuLevel2");
            workstations.SetResourceReference(StyleProperty, "MenuLevel2");
            TabControl.Items.Clear();
            TabControl.Items.Add(categories);
            TabControl.Items.Add(taxes);
            TabControl.Items.Add(units);
            TabControl.Items.Add(suppliers);
            TabControl.Items.Add(workstations);
            TabControl.Items.Add(prices);
            TabControl.Items.Add(products);
            TabControl.Items.Add(recipies);
        }
    }
}
