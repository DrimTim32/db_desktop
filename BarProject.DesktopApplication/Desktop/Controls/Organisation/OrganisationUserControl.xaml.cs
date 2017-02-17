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

namespace BarProject.DesktopApplication.Desktop.Controls.Organisation
{
    using MahApps.Metro.Controls;
    using Warehouse;

    /// <summary>
    /// Interaction logic for OrganisationUserControl.xaml
    /// </summary>
    public partial class OrganisationUserControl : UserControl
    {
        public OrganisationUserControl()
        {
            InitializeComponent();
            SetTabs();
        }
        public void SetTabs()
        {
            var locations = new MetroTabItem()
            {
                Header = "Locations",
                Content = new Locations()
            };

            //var products = new MetroTabItem()
            //{
            //    Header = "Products",
            //   // Content = new Products()
            //};
            //var recipies = new MetroTabItem()
            //{
            //    Header = "Recipies",
            //    //Content = new Recipies()
            //};
            locations.SetResourceReference(StyleProperty, "MenuLevel2");
            //products.SetResourceReference(StyleProperty, "MenuLevel2");
            //recipies.SetResourceReference(StyleProperty, "MenuLevel2");
            TabControl.Items.Clear();
            TabControl.Items.Add(locations);
            //TabControl.Items.Add(products);
            //TabControl.Items.Add(recipies);
        }
    }
}
