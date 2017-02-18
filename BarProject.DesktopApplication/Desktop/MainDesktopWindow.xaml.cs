using System.Windows.Controls;

namespace BarProject.DesktopApplication.Desktop
{
    using Controls;
    using Controls.Menagement;
    using Controls.Organisation;
    using Controls.Warehouse;
    using MahApps.Metro.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainDesktopWindow : MetroWindow
    {
        public MainDesktopWindow()
        {
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
                        tabItem.Content = new UsersUserControl();
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
            var cards = new string[] { "Introduction", "Menagement", "Warehouse", "Administration", "Organisation" };
            foreach (string cardName in cards)
            {
                TabControl.Items.Add(new MetroTabItem() { Header = cardName });
            } 
        }

    }
}
