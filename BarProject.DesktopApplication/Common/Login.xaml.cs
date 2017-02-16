namespace BarProject.DesktopApplication.Common
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;
    using DatabaseProxy.Models;
    using DatabaseProxy.Models.ReadModels;
    using Desktop;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;
    using RestSharp;
    using Utils;

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
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;
            var login = LoginBox.Text;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { LogIn(login, password); }));

        }

        private void LoginError(Exception ex)
        {
            LoginError(ex.Message);
        }
        private void LoginError(string data)
        {
            MessageBoxesHelper.ShowWindowInformation("Problem with login", data);
            ProgressBarStop();
        }


        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(
                () =>
                {
                    Progress.Visibility = Visibility.Visible;
                    this.LoginButton.IsEnabled = false;
                }));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(
                () =>
                {
                    Progress.Visibility = Visibility.Hidden;
                    this.LoginButton.IsEnabled = true;

                }));

        }
        private void ChooseWindow(UserPrivileges privileges)
        {
            if (privileges == UserPrivileges.NoUser)
            {
                LoginError("You do not have enough permissions to log in.");
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

        private void AfterLogin()
        {
           Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(DoAfterLogin));
         
        }
        private void LogIn(string login, string password)
        {
           Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { DoLogin(login, password); }));
        }
        private void DoAfterLogin()
        {
            try
            {
                var text = "";
                text = LoginBox.Text;
                var response = Library.RestHelpers.RestClient.Client().GetUserCredentials(text); 
                if (response.ResponseStatus == ResponseStatus.TimedOut)
                {
                    MessageBoxesHelper.ShowWindowInformation("Request timed out",
                        "Problem connecting to the server");
                }
                else if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBoxesHelper.ShowWindowInformation("Problem with login",
                        "Make sure that both password and login are correct");
                }
                else
                {

                    var privlidges = response.Data;
                    ChooseWindow(privlidges);
                }
                ProgressBarStop();

            }
            catch (Exception ex)
            {
                LoginError(ex);
            }

        }

        private void DoLogin(string login, string password)
        {
            ProgressBarStart();
            try
            {
                Library.RestHelpers.RestClient.Client(ConfigurationManager.AppSettings["apiUrl"])
                    .AutenticateMe(login, password, (response) =>
                    {
                        if (response.ResponseStatus == ResponseStatus.TimedOut)
                        {
                            MessageBoxesHelper.ShowWindowInformation("Request timed out",
                                "Problem connecting to the server");
                        }
                        else if (response.StatusCode != HttpStatusCode.OK)
                        {
                            MessageBoxesHelper.ShowWindowInformation("Problem with login",
                                "Make sure that both password and login are correct");
                        }
                        else
                        {
                            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(AfterLogin));
                        }
                    });

            }
            catch (Exception ex)
            {
                LoginError(ex);
            }

        }
    }
}
