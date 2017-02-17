using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using BarProject.DatabaseProxy.Models.ReadModels;
using MahApps.Metro.Controls;
using BarProject.DesktopApplication.Library.RestHelpers;
namespace BarProject.DesktopApplication.Remote
{
    public partial class CategoriesPage : Page
    {
        private readonly int? _currentId;
        private readonly object idLocker = new object();

        private int? CurrentId
        {
            get
            {
                lock (idLocker) return _currentId;
            }
        }
        private readonly int? _overriding;
        private readonly object overridingIdLocker = new object();

        private int? OverridingId
        {
            get
            {
                lock (overridingIdLocker) return _overriding;
            }
        }
        public CategoriesPage(int? id = null, int? overridingId = null)
        {
            _currentId = id;
            _overriding = overridingId;
            InitializeComponent();
            Loaded += (s, e) => { PageLoaded(); };
        }
        private void PageLoaded()
        {
            AddTiles();
        }

        private void AddBackTile()
        {
            var tile = new Tile() { Title = "BACK" };
            tile.Click += (s, e) =>
            {
                NavigationService.GoBack();
            };
            tile.SetResourceReference(StyleProperty, "LargeTileStyle");
            WrapPanel.Children.Add(tile);
        }

        private class State
        {
            public int Id { get; set; }
            public string Title { get; set; }
        }
        private void AddProductTile(string title, int id)
        {
            var tile = new TileWithId(id) { Title = title };
            tile.SetResourceReference(StyleProperty, "LargeTileStyle");
            tile.Background = Brushes.CadetBlue;
            WrapPanel.Children.Add(tile);
        }
        private void AddCategoryTile(string title, int id)
        {
            var tile = new TileWithId(id) { Title = title };
            tile.SetResourceReference(StyleProperty, "LargeTileStyle");
            tile.Click += (s, e) =>
            {
                NavigationService.Navigate(new CategoriesPage(id));
            };
            WrapPanel.Children.Add(tile);
        }
        private void AddTiles()
        {
            if (WrapPanel.Children.Count != 0)
                return;
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(DoAddTiles));
        }
        private void ProgressBarStart()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Visible));
        }

        private void ProgressBarStop()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Progress.Visibility = Visibility.Hidden));
        }
        private async void DoAddTiles()
        {
            ProgressBarStart();
            AddBackTile();
            int? currId = CurrentId;
            IEnumerable<ShowableCategory> categoriesList;
            if (currId == null)
            {
                var tmp = await RestClient.Client().GetMainCategories();
                categoriesList = tmp.Data;
            }
            else // currId != null 
            {
                var tmp = await RestClient.Client().GetSubCategoriesAsync(currId.Value);
                categoriesList = tmp.Data;
            }
            foreach (var category in categoriesList)
            {
                AddCategoryTile(category.Name, category.Id.Value);
            }
            if (currId != null)
            {
                var tmp = await RestClient.Client().GetProductsByCategory(currId.Value);
                IEnumerable<ShowableSimpleProduct> products = tmp.Data;
                foreach (var product in products)
                {
                    AddProductTile(product.Name, product.Id);
                }
            }
            ProgressBarStop();

        }
    }
}
