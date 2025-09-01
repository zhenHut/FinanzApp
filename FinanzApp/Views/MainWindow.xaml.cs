using System.Reflection;
using System.Windows;

namespace FinanzApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Title = $"FinanzApp v{version?.ToString(3)}";
        }
    }
}