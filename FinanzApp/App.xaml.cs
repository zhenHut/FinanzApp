using AutoUpdaterDotNET;
using FinanzApp.core.Infrastructure;
using FinanzApp.core.Interfaces;
using FinanzApp.core.Notifications;
using FinanzApp.core.Services;
using FinanzApp.Events;
using FinanzApp.Interfaces;
using FinanzApp.Metadata;
using FinanzApp.Security;
using FinanzApp.Services;
using FinanzApp.View;
using FinanzApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;


namespace FinanzApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SQLitePCL.Batteries_V2.Init();

            var appResources = new ResourceDictionary
            {
                Source = new Uri("Resources/Styles.xaml", UriKind.Relative)
            };

            Resources.MergedDictionaries.Add(appResources);

            var culture = CultureInfo.GetCultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(culture.IetfLanguageTag)));

            SecretStore.EnsureDbPassword();

            if (!SecretStore.TryLoad(out var pw))
            {
                MessageBox.Show("Kein DB-Passwort gefunden.", "FinanzApp", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(-1);
                return;
            }

#if DEBUG
            var appDirName = "FinanzApp-Dev";
            var dbFileName = "finanzapp.dev.db";
#else
            var appDirName = "FinanzApp";
            var dbFileName = "finanzapp.db";
#endif

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var dbDir = Path.Combine(appData, appDirName);
            Directory.CreateDirectory(dbDir);
            var dbPath = Path.Combine(dbDir, dbFileName);
            var cs = $"Data Source={dbPath}; Password={pw}";

            var services = new ServiceCollection();
            services.AddDbContext<FinanzAppDbContext>(options
                => options.UseSqlite(cs, builder => builder.MigrationsAssembly(typeof(FinanzAppDbContext).Assembly.GetName().Name)));

            services.AddScoped<ITransactionService, TransactionServices>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainWindow>(sp => new MainWindow
            {
                DataContext = sp.GetRequiredService<MainViewModel>()
            });

            Services = services.BuildServiceProvider();

            var notifier = Services.GetRequiredService<INotificationService>();
            notifier.Notified += HandleNotification;

            using (var scope = Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<FinanzAppDbContext>();
                ctx.Database.Migrate();
            }

            AutoUpdater.AppTitle = AppInfo.Product;
            AutoUpdater.InstalledVersion = AppInfo.SemVerVersion;

            AutoUpdater.Start("https://zhenhut.github.io/FinanzAppUpdates/update.xml");

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Title = $"{AutoUpdater.AppTitle}";
            MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void HandleNotification(object? sender, NotificationEventArgs e)
        {
            var notification = e.Notification;
            Dispatcher.Invoke(() =>
            {
                switch (notification.Kind)
                {
                    case NotificationType.Error:
                        MessageBox.Show(notification.Message, notification.Title ?? "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;

                    case NotificationType.Warning:
                        MessageBox.Show(notification.Message, notification.Title ?? "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;

                    default:
                        //TODO: Statusbar/Snackbar
                        break;
                }
            });
        }

        //static string GetProductVersionString()
        //{
        //    // Bevorzugt die InformationalVersion (entspricht <Version>)
        //    var info = Assembly.GetExecutingAssembly()
        //        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        //    if (!string.IsNullOrWhiteSpace(info))
        //        return info.Split('+')[0].Split('-')[0]; // z.B. "1.3.0"

        //    // Fallback: FileVersion
        //    return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion ?? "1.0.0.0";
        //}

        //static Version GetProductVersion() => Version.Parse(GetProductVersionString());
    }

}
