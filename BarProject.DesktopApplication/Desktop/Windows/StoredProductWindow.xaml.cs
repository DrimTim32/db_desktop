﻿using System;
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
using System.Windows.Shapes;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Threading;
    using DatabaseProxy.Functions;
    using DatabaseProxy.Models.ReadModels;
    using Library.RestHelpers;

    /// <summary>
    /// Interaction logic for StoredProductWindow.xaml
    /// </summary>
    public partial class StoredProductWindow : Window
    {
        private object productIdLocker = new object();
        private object simpleproductLocker = new object();

        private int productId
        {
            get
            {
                lock (productIdLocker) return _productId;
            }
        }

        private ShowableSimpleProduct storedProduct;
        private readonly int _productId;

        public ShowableSimpleProduct StoredProduct
        {
            get
            {
                lock (simpleproductLocker)
                    return storedProduct;
            }
            set
            {
                lock (simpleproductLocker)
                    storedProduct = value;
            }
        }
        public StoredProductWindow(int id)
        {
            _productId = id;
            Loaded += StoredProductWindow_Loaded; ;
            InitializeComponent();
        }

        private void StoredProductWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadModel();
        }

        private void ReloadModel()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoRefreshData));
        }
        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        private async void DoRefreshData()
        {
            ProgressBarStart();
            var tmp = await RestClient.Client().GetStoredProduct(productId);
            StoredProduct = tmp.Data;
            ProgressBarStop();
        }
    }
}
