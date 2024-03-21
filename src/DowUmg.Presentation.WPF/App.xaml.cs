using System.Windows;
using DowUmg.Platform;
using DowUmg.Presentation.Handlers;
using DowUmg.Presentation.WPF.Platform;
using DowUmg.Presentation.WPF.Views;
using DowUmg.Services;
using ReactiveUI;
using Splat;

namespace DowUmg.Presentation.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IEnableLogger
    {
        public App()
        {
            InitializeComponent();
        }

        private void OnApplicationStartup(object sender, StartupEventArgs args)
        {
            RxApp.DefaultExceptionHandler = new DefaultExceptionHandler();

            RegisterDependencies();

            MigrateDatabase();

            var window = new MainWindow();
            window.Closed += delegate
            {
                Shutdown();
            };
            window.Show();
        }

        private void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterLazySingleton<IViewLocator>(() => new AppViewLocator());
            Locator.CurrentMutable.RegisterLazySingleton<IFilePathProvider>(
                () => new WindowsFilePathProvider()
            );
            AppBootstrapper.RegisterDefaults();
        }

        private void MigrateDatabase()
        {
            Locator.Current.GetService<IModDataService>()?.MigrateData();
        }
    }
}
