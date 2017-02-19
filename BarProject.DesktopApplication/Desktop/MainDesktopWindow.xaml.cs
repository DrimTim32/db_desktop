using System.Windows.Controls;
using BarProject.DatabaseProxy.Models;
using BarProject.DesktopApplication.Desktop.Controls.Administration;

namespace BarProject.DesktopApplication.Desktop
{
    using Controls.Menagement;
    using Controls.Organisation;
    using Controls.Warehouse;
    using MahApps.Metro.Controls;
    public partial class MainDesktopWindow : MetroWindow
    {
        private readonly UserPrivileges privileges;
        public MainDesktopWindow(UserPrivileges privileges)
        {
            this.privileges = privileges;
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            SetCards();
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabItem = (((TabControl)sender).SelectedItem as TabItem);
            if (tabItem.Content == null)
            {
                switch ((string)tabItem.Header)
                {
                    case "Introduction":
                        tabItem.Content = new Introduction();
                        break;

                    case "Menagement":
                        tabItem.Content = new MenagementUserControl();
                        break;

                    case "Warehouse":
                        tabItem.Content = new WarehouseUserControl();
                        break;

                    case "Administration":
                        tabItem.Content = new AdministrationUserControl();
                        break;

                    case "Organisation":
                        tabItem.Content = new OrganisationUserControl();
                        break;

                    default:
                        return;
                }
            }
        }

        private void SetCards()
        {
            var cards = new string[] { "Introduction", "Menagement", "Warehouse", "Organisation", "Administration" };
            if (privileges < UserPrivileges.Admin)
                cards = new string[] { "Introduction", "Menagement", "Warehouse" };
            foreach (string cardName in cards)
            {
                TabControl.Items.Add(new MetroTabItem() { Header = cardName });
            }
        }

    }
}
