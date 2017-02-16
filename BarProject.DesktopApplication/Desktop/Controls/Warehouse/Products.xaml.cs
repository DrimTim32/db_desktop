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

namespace BarProject.DesktopApplication.Desktop.Controls.Warehouse
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Threading;
    using Windows;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ReadModels;
    using Library.RestHelpers;

    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : UserControl
    {
        private ObservableCollection<ShowableSimpleProduct> _productsList;
        private readonly object ShowableSimpleProductLock = new object();

        public ObservableCollection<ShowableSimpleProduct> ProductLists
        {
            get
            {
                lock (ShowableSimpleProductLock)
                {
                    if (_productsList == null)
                        _productsList =
                            new ObservableCollection<ShowableSimpleProduct>();
                    return _productsList;
                }
            }
            set { lock (ShowableSimpleProductLock) { _productsList = value; } }
        }
        public Products()
        {
            InitializeComponent();
            DataGrid.MouseDoubleClick += DataGrid_MouseDoubleClick;
            Loaded += Products_Loaded;
        }

        private void Products_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));

        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        private void RefreshData()
        {

            this.Dispatcher.Invoke(DispatcherPriority.Background,
                  new Action(
                  DoRefreshData
                  ));
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetProducts();
            ProductLists.Clear();
            foreach (var showableTax in tmp.Data)
            {
                ProductLists.Add(showableTax);
            }
            DataGrid.Items.Refresh();
            ProgressBarStop();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg != null)
            {
                var dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));
                var product = (ShowableSimpleProduct)dgr.Item;
                if (product.IsSold)
                {

                    var window = new SoldProductDataWindow(product.Id);
                    window.Closed += (s, x) => RefreshData();
                    window.ShowDialog();
                }
                else if (product.IsStored)
                {
                    var window = new StoredProductWindow(product.Id);
                    window.Closed += (s, x) => RefreshData();
                    window.ShowDialog();
                } 
            }
        }
    }
}
