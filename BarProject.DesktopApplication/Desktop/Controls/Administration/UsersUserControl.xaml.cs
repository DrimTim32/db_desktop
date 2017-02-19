using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using BarProject.DesktopApplication.Library.RestHelpers;

namespace BarProject.DesktopApplication.Desktop.Controls.Administration
{
    /// <summary>
    /// Interaction logic for AdministrationUserControl.xaml
    /// </summary>
    public partial class UsersUserControl : UserControl
    {
        public UsersUserControl()
        {
            InitializeComponent();
            Loaded += AllUsers_Loaded;
            Progress.Visibility = Visibility.Hidden;
        }
        private void AllUsers_Loaded(object sender, RoutedEventArgs e)
        {

            Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(
                  DoLoadAllUsers
                  ));
        }

        private async void DoLoadAllUsers()
        {
            ProgressBarStart();
            var users = await RestClient.Client().GetUsers();

            await Dispatcher.InvokeAsync(() =>
             {
                 DataGrid.DataContext = users.Data;
             });
            ProgressBarStop();
        }

        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));

        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }

        private void AddNewUserButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new Windows.AddUserWindow();
            window.Closed += (o, args) => DoLoadAllUsers();
            window.ShowDialog();

        }

    }
}