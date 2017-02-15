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

namespace BarProject.DesktopApplication.Desktop.Controls
{
    using System.Threading;
    using Library.RestHelpers;

    /// <summary>
    /// Interaction logic for AdministrationUserControl.xaml
    /// </summary>
    public partial class UsersUserControl : System.Windows.Controls.UserControl
    {
        public UsersUserControl()
        {
            InitializeComponent();
            Loaded += AllUsers_Loaded;
            Progress.Visibility = Visibility.Hidden;
        }
        private void AllUsers_Loaded(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(DoLoadAllUsers);
            thread.Start();
        }

        private async void DoLoadAllUsers()
        {
            var users = await RestClient.Client().GetUsers();

            await Dispatcher.InvokeAsync(() =>
             {
                 DataGrid.DataContext = users.Data;
                 Progress.Visibility = Visibility.Hidden;
             });
        }

        private void AddNewUserButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new Windows.AddUserWindow();
            window.ShowDialog();
        }
    }
}
