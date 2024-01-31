using System.Windows;
using DowUmg.Data;
using DowUmg.Interfaces;
using DowUmg.Presentation.Handlers;
using DowUmg.Presentation.ViewModels;
using DowUmg.Presentation.WPF.Pages;
using DowUmg.Presentation.WPF.Services;
using Microsoft.EntityFrameworkCore;
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
            RegisterViews();

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
            Locator.CurrentMutable.RegisterLazySingleton<IFilePathProvider>(
                () => new WindowsFilePathProvider()
            );
            AppBootstrapper.RegisterDefaults();
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register<IViewFor<TitleViewModel>>(() => new TitlePage());
            Locator.CurrentMutable.Register<IViewFor<SettingsViewModel>>(() => new SettingsPage());
            Locator.CurrentMutable.Register<IViewFor<ModsViewModel>>(() => new ModsPage());
            Locator.CurrentMutable.Register<IViewFor<GenerationViewModel>>(
                () => new GenerationSettingsPage()
            );
            Locator.CurrentMutable.Register<IViewFor<MatchupViewModel>>(() => new MatchupPage());
        }

        private void MigrateDatabase()
        {
            using var context = new ModsContext();
            context.Database.Migrate();
        }
    }
}
