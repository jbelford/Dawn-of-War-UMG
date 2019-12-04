using DowUmg.Data;
using DowUmg.Interfaces;
using DowUmg.Presentation.WPF.Services;
using DowUmg.Presentation.WPF.Views;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using Splat;
using System.Reflection;
using System.Windows;

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
            RegisterDependencies();
            MigrateDatabase();
        }

        private void OnApplicationStartup(object sender, StartupEventArgs args)
        {
            var window = new RoutingWindow();
            window.Closed += delegate { Shutdown(); };
            window.Show();
        }

        private void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterLazySingleton<IFilePathProvider>(() => new WindowsFilePathProvider());
            AppBootstrapper.RegisterDefaults();
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        }

        private void MigrateDatabase()
        {
            using var context = new DataContext();
            context.Database.Migrate();
        }
    }
}
