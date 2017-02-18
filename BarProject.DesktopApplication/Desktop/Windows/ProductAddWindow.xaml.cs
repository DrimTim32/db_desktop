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
using System.Windows.Shapes;
using BarProject.DatabaseProxy.Models.ReadModels;
using BarProject.DatabaseProxy.Models.WriteModels;

namespace BarProject.DesktopApplication.Desktop.Windows
{
    /// <summary>
    /// Interaction logic for ProductAddWindow.xaml
    /// </summary>
    public partial class ProductAddWindow : Window
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
        public ProductAddWindow()
        {
            InitializeComponent(); 
        }
    }
}
