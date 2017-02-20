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
using MahApps.Metro.Controls;

namespace BarProject.DesktopApplication.Desktop.Controls.Administration
{
    /// <summary>
    /// Interaction logic for AdministrationUserControl.xaml
    /// </summary>
    public partial class AdministrationUserControl : UserControl
    {
        public AdministrationUserControl()
        {
            InitializeComponent();
            SetTabs();
        }

        private void SetTabs()
        {
            var users = new MetroTabItem()
            {
                Header = "Users",
                Content = new UsersUserControl()
            };
            var logs = new MetroTabItem()
            {
                Header = "Logs",
                Content = new LoginLogs()
            };
            users.SetResourceReference(StyleProperty, "MenuLevel2");
            logs.SetResourceReference(StyleProperty, "MenuLevel2");
            TabControl.Items.Clear();
            TabControl.Items.Add(users);
            TabControl.Items.Add(logs);
        }
    }
}
