namespace BarProject.DesktopApplication.Common
{
    using System;
    using System.Configuration;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using DatabaseProxy.Models;
    using DatabaseProxy.Models.ReadModels;
    using Desktop;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;

    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        public Login()
        {
            InitializeComponent();
            LoginBox.Text = "malin";
            PasswordBox.Password = "qwerty";
            ProgressReport.Visibility = Visibility.Hidden;
            Progress.IsIndeterminate = true;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;
            var login = LoginBox.Text;
            LogIn(login, password);
        }

        private async void LoginError()
        {
            await Dispatcher.InvokeAsync(() =>
            {
                this.ShowMessageAsync("Problem with login.", "Please, check your login and/or password"); 
                ProgressReport.Visibility = Visibility.Hidden;
            });
        }

        private void ChooseWindow(UserPrivileges privileges)
        {
            if (privileges == UserPrivileges.NoUser)
            {
                LoginError();
            }
            else if (privileges < UserPrivileges.WarehouseAdministrator)
            {
                //TODO: bar app
            }
            else
            {
                var window = new MainDesktopWindow { ShowActivated = true };
                Application.Current.Dispatcher.Invoke(() =>
                { 
                    Application.Current.MainWindow = window;
                    window.Show();
                });
                Close();
            }
        }

        private async void DoAfterLogin()
        {
            try
            {
                var text = "";
                Dispatcher.Invoke(() =>
                {
                    text = LoginBox.Text;
                });
                var test = await Library.RestHelpers.RestClient.Client().GetUserCredentials(text);

                await Dispatcher.InvokeAsync(() =>
                {
                    ProgressReport.Visibility = Visibility.Hidden;
                    ChooseWindow(test.Data);
                });
            }
            catch (Exception ex)
            {

                LoginError();
            }

        }
        private void LogIn(string login, string password)
        {
            ProgressReport.Visibility = Visibility.Visible;
            var thread = new Thread(() => { DoLogin(login, password); });
            thread.Start();
        }

        private void AfterLogin()
        {
            var thread = new Thread(DoAfterLogin);
            thread.Start();
        }
        private async void DoLogin(string login, string password)
        {
            try
            {
                Library.RestHelpers.RestClient.Client(ConfigurationManager.AppSettings["apiUrl"])
                    .AutenticateMe(login, password);
                await Dispatcher.InvokeAsync(AfterLogin);
            }
            catch (Exception ex)
            {
                LoginError();
            }

        }
    }
}
