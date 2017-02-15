using System;
using System.Windows;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    using DatabaseProxy.Models;
    using MahApps.Metro.Controls;

    public partial class AddUserWindow : MetroWindow
    {
        public AddUserWindow()
        {
            InitializeComponent();
            ComboPermission.ItemsSource = UserPrivlidgesExtensions.GetManagable;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var usr = User;
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
            usr.Password = TextPassword.Password;
            if (usr.Error != null)
                MessageBox.Show(usr.Error);
            else
            {
                this.Close();
            }
        }
    }
}
