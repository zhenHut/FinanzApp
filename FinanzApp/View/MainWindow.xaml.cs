using FinanzApp.core.Interface;
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
            Loaded += EventloadingNotifyRequest;
            
            var version = Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void EventloadingNotifyRequest(object sender, EventArgs e)
        {
            if (DataContext is INotificationRequest notifier)
            {
                notifier.NotificationRequested += OnMessageSending;
            }
        }

        private  void OnMessageSending(object? sender, string msg)
        {
                MessageBox.Show(msg, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
   
        }
    }
}