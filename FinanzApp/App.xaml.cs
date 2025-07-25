using FinanzApp.core.Data;
using FinanzApp.core.Interface;
using FinanzApp.core.Services;
using FinanzApp.core.ViewModel;
using FinanzApp.View;
using System.Configuration;
using System.Data;
using System.Windows;


namespace FinanzApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var appResources = new ResourceDictionary
            {
                Source = new Uri("Resources/Styles.xaml", UriKind.Relative)
            };

            Resources.MergedDictionaries.Add(appResources);


            IDialogService navigationService = new DialogService();
            FinanzAppDbContext finanzAppDbContext = new FinanzAppDbContext();
            ITransactionService transactionService = new TransactionServices(finanzAppDbContext);
            var mainViewModel = new MainViewModel(transactionService ,navigationService);

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel,
            };

            mainWindow.Show();
        }
    }

}
