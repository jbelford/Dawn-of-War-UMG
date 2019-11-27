using DowUmg.Interfaces;
using DowUmg.Presentation.WPF.Services;
using DowUmg.Presentation.WPF.Views;
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
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            Locator.CurrentMutable.RegisterLazySingleton<IFilePathProvider>(() => new WindowsFilePathProvider());
            AppBootstrapper.RegisterDefaults();
        }

        private void MigrateDatabase()
        {
            //DbContext context = Locator.Current.GetService<DbContext>();
            //context.Database.Migrate();
        }
    }
}
