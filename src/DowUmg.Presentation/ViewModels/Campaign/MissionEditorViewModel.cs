using System;
using System.Reactive;
using DowUmg.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class MissionEditorViewModel : RoutableReactiveObject
    {
        public MissionEditorViewModel(IScreen screen, MissionListItemViewModel missionViewModel)
            : base(screen, "missionEdit")
        {
            CancelCommand = HostScreen.Router.NavigateBack;

            Name = missionViewModel.Mission.Name;
            Description = missionViewModel.Mission.Description;

            SaveCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                missionViewModel.Mission = new CampaignMission(missionViewModel.Mission.Map)
                {
                    Name = Name,
                    Description = Description,
                };
                return CancelCommand.Execute();
            });
        }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string Description { get; set; }

        public ReactiveCommand<Unit, IRoutableViewModel> CancelCommand { get; set; }
        public ReactiveCommand<Unit, IRoutableViewModel> SaveCommand { get; set; }
    }
}
