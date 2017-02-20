using System.Text.RegularExpressions;

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
    using Remote;
    using RestSharp;
    using Utils;

    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        private int SpotId { get; }
        private int WorkstationId { get; }
        public Login()
        {
            InitializeComponent();
            LoginBox.Text = "malin";
            PasswordBox.Password = "qwerty";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SpotId = ConfigHelper.Spot;
            WorkstationId = ConfigHelper.Workstation;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;
            var login = LoginBox.Text;
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { LogIn(login, password); }));

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
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(
                () =>
                {
                    Progress.Visibility = Visibility.Visible;
                    this.LoginButton.IsEnabled = false;
                }));
        }

        private void ProgressBarStop()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(
                () =>
                {
                    Progress.Visibility = Visibility.Hidden;
                    this.LoginButton.IsEnabled = true;

                }));

        }
        private void ChooseWindow(UserPrivileges privileges, string userName)
        {
            if (privileges == UserPrivileges.NoUser)
            {
                LoginError("You do not have enough permissions to log in.");
            }
            else if (privileges < UserPrivileges.Warehouse)
            {
                var window = new MainRemoteWindow(userName) { ShowActivated = true };
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Application.Current.MainWindow = window;
                    window.Show();
                });
                Close();
            }
            else
            {
                var window = new MainDesktopWindow(privileges) { ShowActivated = true };
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Application.Current.MainWindow = window;
                    window.Show();
                });
                Close();
            }
        }

        private void LogIn(string login, string password)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { DoLogin(login, password); }));
        }
        Regex dataRegex = new Regex(@"""error_description"":""(.*?)""");
        private void DoLogin(string login, string password)
        {
            ProgressBarStart();
            try
            {
                Library.RestHelpers.RestClient.Client(ConfigurationManager.AppSettings["apiUrl"])
                    .AutenticateMe(login, password, SpotId, WorkstationId, (response, privlidges) =>
                     {
                         if (response.ResponseStatus == ResponseStatus.TimedOut)
                         {
                             MessageBoxesHelper.ShowProblemWithRequest(response);
                             Application.Current.Dispatcher.Invoke(ProgressBarStop);
                         }
                         else if (response.ResponseStatus == ResponseStatus.Completed &&
                                    response.StatusCode == HttpStatusCode.BadRequest)
                         {
                             var match = dataRegex.Match(response.Content);
                             MessageBoxesHelper.ShowWindowInformation("Forbidden", match.Groups[1].Value);
                             Application.Current.Dispatcher.Invoke(ProgressBarStop);
                         }

                         else if (response.StatusCode != HttpStatusCode.OK)
                         {
                             MessageBoxesHelper.ShowProblemWithRequest(response);
                             Application.Current.Dispatcher.Invoke(ProgressBarStop);
                         }
                         else
                         {
                             Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { ChooseWindow(privlidges, login); }));
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
