﻿using System;
using System.Linq;
using System.Reactive;
using DowUmg.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class TitleViewModel : RoutableReactiveObject
    {
        public TitleViewModel(IScreen screen)
            : base(screen, "main")
        {
            IModDataService modDataService = Locator.Current.GetService<IModDataService>()!;
            IsLoaded = modDataService.GetPlayableMods().Any();

            SettingsAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen))
            );
            ModsAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new ModsViewModel(HostScreen))
            );
            MatchupAction = ReactiveCommand.CreateFromObservable(
                () =>
                    HostScreen.Router.Navigate.Execute(new GenerationSettingsViewModel(HostScreen))
            );

            CampaignAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new CampaignViewModel(HostScreen))
            );

            CloseApp = ReactiveCommand.Create(() => Environment.Exit(0));
        }

        public ReactiveCommand<Unit, Unit> CloseApp { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> SettingsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> ModsAction { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> MatchupAction { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> CampaignAction { get; }

        [Reactive]
        public bool IsLoaded { get; set; }
    }
}
