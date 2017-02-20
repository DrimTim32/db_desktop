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
    public partial class ProductsUsage : UserControl
    {
        private ObservableCollection<ShowableProductUsage> _categories;
        private readonly object PossibleOverridingCategoriesLock = new object();
        public ObservableCollection<ShowableProductUsage> UsagesList
        {
            get
            {
                lock (PossibleOverridingCategoriesLock)
                {
                    if (_categories == null)
                        _categories = new ObservableCollection<ShowableProductUsage>();
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
        public ProductsUsage()
        {
            InitializeComponent();
            Loaded += ProductsUsage_Loaded;
        }

        private void ProductsUsage_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoLoadOrders));
        }
        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));

        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        private async void DoLoadOrders()
        {
            ProgressBarStart();
            var users = await RestClient.Client().GetProductsUsage();

            await Dispatcher.InvokeAsync(() =>
            {
                UsagesList.Clear();
                var tmp = users.Data;
                foreach (var showableClientOrder in tmp)
                {
                    UsagesList.Add(showableClientOrder);
                }
            });
            ProgressBarStop();
        }

    }
}
