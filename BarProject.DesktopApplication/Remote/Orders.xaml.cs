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
using BarProject.DatabaseProxy.Models.WriteModels;
using BarProject.DesktopApplication.Common.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BarProject.DesktopApplication.Remote
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        public Orders()
        {
            InitializeComponent();
            this.Loaded += Orders_Loaded;
        }

        private void Orders_Loaded(object sender, RoutedEventArgs e)
        {

            RefreshTiles();
        }

        private void AddBackTile()
        {
            var tile = new Tile { Title = "BACK" };
            tile.Click += (s, e) =>
            {
                NavigationService.GoBack();
            };
            tile.SetResourceReference(StyleProperty, "LargeTileStyle");
            WrapPanel.Children.Add(tile);
        }
        private void AddOrderTile(WritableOrder order)
        {
            var tile = new Tile { Title = order.Name };
            tile.SetResourceReference(StyleProperty, "LargeTileStyle");
            tile.Click += (s, e) =>
            {
                var data = MessageBoxesHelper.ShowYesNoMessage("Are you sure?", "Mark this order as paid");
                if (data == MessageDialogResult.Affirmative)
                {
                    var window = Window.GetWindow(this) as MainRemoteWindow;
                    window.SetAsPaid(order);
                    RefreshTiles();
                }
            };
            WrapPanel.Children.Add(tile);
        }
        public void RefreshTiles()
        {
            WrapPanel.Children.Clear(); ;
            AddBackTile();
            foreach (var order in MainRemoteWindow.OrderToDo)
            {
                AddOrderTile(order);
            }
        }
    }
}
