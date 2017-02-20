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
using System.Windows.Threading;
using BarProject.DesktopApplication.Library.RestHelpers;

namespace BarProject.DesktopApplication.Desktop.Controls.Administration
{
    /// <summary>
    /// Interaction logic for LoginLogs.xaml
    /// </summary>
    public partial class LoginLogs : UserControl
    {
        public LoginLogs()
        {
            InitializeComponent();
            Loaded += LoginLogs_Loaded;
        }

        private void LoginLogs_Loaded(object sender, RoutedEventArgs e)
        {

            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoLoadLogs));
        }

        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));

        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }

        private async void DoLoadLogs()
        {
            ProgressBarStart();
            var logs = await RestClient.Client().GetLogs();

            await Dispatcher.InvokeAsync(() =>
            {
                DataGrid.DataContext = logs.Data;
            });
            ProgressBarStop();
        }
    }
}
