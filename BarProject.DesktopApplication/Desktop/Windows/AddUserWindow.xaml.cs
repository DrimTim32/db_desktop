using System;
using System.Windows;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Threading;
    using DatabaseProxy.Models;
    using MahApps.Metro.Controls;
    using RestSharp;
    using RestClient = Library.RestHelpers.RestClient;

    public partial class AddUserWindow : MetroWindow
    {
        public AddUserWindow()
        {
            InitializeComponent();
            ComboPermission.ItemsSource = UserPrivlidgesExtensions.GetManagable;
        }

        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));

        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }

        public void RanToCompletion()
        {
            if (this.Dispatcher.CheckAccess())
                this.Close();
            else
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(this.Close));
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextPassword.Password == null)
            {
                MessageBox.Show("Password is required.");
                return;
            }
            if (TextPassword.Password.Length < 3)
            {
                MessageBox.Show("Password is too short.");
                return;
            }
            User.Password = TextPassword.Password;
            if (User.Error != null)
                MessageBox.Show(User.Error);
            else
            {
                ProgressBarStart();
                Dispatcher.Invoke(() =>
                {
                    RestClient.Client().AddUser(User.ToPure(), (response, handle) =>
                    {
                        ProgressBarStop();
                        if (response.ResponseStatus == ResponseStatus.Completed &&
                            response.StatusCode == HttpStatusCode.OK)
                        {
                            MessageBox.Show("Success!");
                            RanToCompletion();
                        }
                        else
                        {
                            MessageBox.Show("ERROR??" + response.ErrorMessage);
                        }
                    });
                });
            }
        }
    }
}
