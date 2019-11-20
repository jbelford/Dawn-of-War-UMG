using DowUmg.Presentation.WPF.Models;
using DowUmg.Presentation.WPF.Views;
using DowUmg.Services;
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
            Locator.CurrentMutable.RegisterLazySingleton(() => new DataLoader());
            Locator.CurrentMutable.RegisterLazySingleton(() => new DowPathService());
            Locator.CurrentMutable.RegisterLazySingleton(() => new AppSettingsService());
        }
    }
}
