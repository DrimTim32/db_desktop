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

    /// <summary>
    /// Interaction logic for MenagementUserControl.xaml
    /// </summary>
    public partial class MenagementUserControl : System.Windows.Controls.UserControl
    {
        public MenagementUserControl()
        {
            InitializeComponent();
            SetTabs();
        } 

        public void SetTabs()
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
            categories.SetResourceReference(StyleProperty, "MenuLevel2");
            taxes.SetResourceReference(StyleProperty, "MenuLevel2");
            units.SetResourceReference(StyleProperty, "MenuLevel2");
            TabControl.Items.Clear();
            TabControl.Items.Add(categories);
            TabControl.Items.Add(taxes);
            TabControl.Items.Add(units);
        }
    }
}
