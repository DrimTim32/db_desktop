using System.Windows;

namespace BarProject.DesktopApplication.Remote
{

    public partial class OrderCodeWindow
    {
        public string OrderId { get; set; } = null;
        public OrderCodeWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBlock.Text))
                return;
            OrderId = TextBlock.Text;
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        { 
            Close();
        }
    }
}
