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

namespace BarProject.DesktopApplication.Desktop
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Introduction : UserControl
    {
        public Introduction()
        {
            InitializeComponent();
            Loaded += Introduction_Loaded;
        }

        private void Introduction_Loaded(object sender, RoutedEventArgs e)
        {
            string appDir = Environment.CurrentDirectory;
            Uri pageUri = new Uri(appDir + "/Resources/Introduction.html");
            Browser.Source = pageUri;
        }
    }
}
