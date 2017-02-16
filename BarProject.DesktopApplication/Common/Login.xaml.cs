namespace BarProject.DesktopApplication.Common
{
    using System;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
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
        static Regex tokenRegex = new Regex("\"access_token\":\"(.*?)\"", RegexOptions.Multiline);
        private void DoLogin(string login, string password)
        {
            try
            {
                Library.RestHelpers.RestClient.Client(ConfigurationManager.AppSettings["apiUrl"])
                    .AutenticateMe(login, password, (resp) =>
                    {
                        if (resp.ResponseStatus == ResponseStatus.TimedOut)
                        {
                            MessageBoxesHelper.ShowWindowInformation("Request timed out",
                                "Problem connecting to the server");
                        }
                        else if (resp.StatusCode != HttpStatusCode.OK)
                        {
                            MessageBoxesHelper.ShowWindowInformation("Problem with login",
                                "Make sure that both password and login are correct"); 
                        }
                        else
                        {
                            DoAfterLogin();
                        }
                    });
            }
            catch (Exception ex)
            {
                LoginError();
            }

        }
    }
}
