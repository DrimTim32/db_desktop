using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using BarProject.DatabaseProxy.Models.ReadModels;

namespace BarProject.DesktopApplication.Remote
{
    public class ConfigurationData : INotifyPropertyChanged
    {
        private short _quantity;
        public short Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
        }

        //below is the boilerplate code supporting PropertyChanged events:
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
    public partial class ProductPage : Page
    {
        public ShowableSoldProduct Product { get; set; }
        public ConfigurationData Data { get; set; } = new ConfigurationData();
        public ProductPage(ShowableSoldProduct product)
        {
            this.Product = product;
            InitializeComponent();
            TileUp.Click += TileUp_Click;
            TileDown.Click += TileDown_Click;
        }

        private void TileDown_Click(object sender, RoutedEventArgs e)
        {
            if (Data.Quantity >= 1)
                Data.Quantity -= 1;
        }

        private void TileUp_Click(object sender, RoutedEventArgs e)
        {
            Data.Quantity += 1;
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this) as MainRemoteWindow;
            if (Data.Quantity != 0)
                window?.RegisterProduct(Product, Data.Quantity);
            NavigationService.GoBack();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
