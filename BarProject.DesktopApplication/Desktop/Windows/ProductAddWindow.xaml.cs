﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Threading;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.WriteModels;
using BarProject.DesktopApplication.Common.Utils;
using MahApps.Metro.Controls;
using RestSharp;
using RestClient = BarProject.DesktopApplication.Library.RestHelpers.RestClient;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    public partial class ProductAddWindow : MetroWindow
    {
        private readonly object newProductLocker = new object();
        private WritableProduct _soldProduct;
        public WritableProduct WritableProduct
        {
            get
            {
                lock (newProductLocker)
                {
                    if (_soldProduct == null)
                        _soldProduct = new WritableProduct();
                    return _soldProduct;
                }
            }
            set
            {
                lock (newProductLocker) _soldProduct = value;
            }
        }
        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        public ProductAddWindow(IEnumerable<string> taxNames, IEnumerable<string> categoriesNames, IEnumerable<string> unitsNames, IEnumerable<string> reciptNames)
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                TextTaxName.ItemsSource = taxNames;
                TextUnitName.ItemsSource = unitsNames;
                TextCategoryName.ItemsSource = categoriesNames;
                TextRecipitName.ItemsSource = reciptNames;
            };
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ProgressBarStart();
            if (SoldSwitch.IsChecked != null)
            {
                WritableProduct.IsSold = SoldSwitch.IsChecked.Value;
            }
            else
            {
                WritableProduct.IsSold = false;
            }
            if (StoredSwitch.IsChecked != null)
            {
                WritableProduct.IsStored = StoredSwitch.IsChecked.Value;
            }
            else
            {
                WritableProduct.IsStored = false;
            }
            RestClient.Client().AddProduct(WritableProduct,
                 (response, handle) =>
                 {
                     if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
                     {
                         MessageBoxesHelper.ShowWindowInformationAsync("Problem with writing to database",
                             response.Content, this);
                         ProgressBarStop();
                     }
                     else
                     {
                         MessageBox.Show("Success");
                         Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(Close));
                     }
                 });
        }
    }
}
