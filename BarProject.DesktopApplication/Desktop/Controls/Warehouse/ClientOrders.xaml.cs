using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DesktopApplication.Library.RestHelpers;

namespace BarProject.DesktopApplication.Desktop.Controls.Warehouse
{
    public partial class ClientOrders : UserControl
    {
        private ObservableCollection<ShowableClientOrder> _categories;
        private object PossibleOverridingCategoriesLock = new object();
        public ObservableCollection<ShowableClientOrder> CategoriesList
        {
            get
            {
                lock (PossibleOverridingCategoriesLock)
                {
                    if (_categories == null)
                        _categories = new ObservableCollection<ShowableClientOrder>();
                    return _categories;
                }
            }
            set
            {

                lock (PossibleOverridingCategoriesLock)
                {
                    _categories = value;
                }
            }
        }
        public ClientOrders()
        {
            InitializeComponent();
            Loaded += ClientOrders_Loaded;
        }

        private void ClientOrders_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoLoadOrders));
        }

        private async void DoLoadOrders()
        {
            ProgressBarStart();
            var users = await RestClient.Client().GetClientOrders();

            await Dispatcher.InvokeAsync(() =>
            {
                CategoriesList.Clear();
                var tmp = users.Data.Where(x => x.PaymentTime.HasValue);
                foreach (var showableClientOrder in tmp)
                {
                    CategoriesList.Add(showableClientOrder);
                }
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

    }
}
