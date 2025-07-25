using FinanzApp.core.Interface;
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
            Loaded += (s, e) =>
            {

                if (DataContext is INotificationRequest notifier)
                {
                    notifier.NotificationRequested += (s, msg) =>
                    {
                        MessageBox.Show(msg, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    };
                }
            };
        }
    }
}