namespace BarProject.DesktopApplication.Desktop
{
    using Controls;
    using Controls.Menagement;
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
        private void SetCards()
        {
            var initialisation = new MetroTabItem
            {
                Header = "Introduction",
            };
            var menagement = new MetroTabItem
            {
                Header = "Menagement",
                Content = new MenagementUserControl()
            };

            var warehouse = new MetroTabItem
            {
                Header = "Warehouse",
                Content= new WarehouseUserControl()
            };
            var administration = new MetroTabItem
            {
                Header = "Administration",
                Content = new UsersUserControl()
            };

            TabControl.Items.Add(initialisation);
            TabControl.Items.Add(menagement);
            TabControl.Items.Add(warehouse);
            TabControl.Items.Add(administration);
        }

    }
}
