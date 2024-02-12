using System;
using System.Reactive;
using DowUmg.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class MissionEditorViewModel : RoutableReactiveObject
    {
        public MissionEditorViewModel(
            IScreen screen,
            CampaignMission mission,
            Action<CampaignMission> onSave
        )
            : base(screen, "missionEdit")
        {
            CancelCommand = HostScreen.Router.NavigateBack;

            Name = mission.Name;
            Description = mission.Description;
            Map = mission.Map;

            SaveCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                onSave(CreateMission());
                return CancelCommand.Execute();
            });
        }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string Description { get; set; }

        [Reactive]
        public CampaignMap Map { get; set; }

        public ReactiveCommand<Unit, IRoutableViewModel> CancelCommand { get; set; }
        public ReactiveCommand<Unit, IRoutableViewModel> SaveCommand { get; set; }

        private CampaignMission CreateMission() =>
            new(Map) { Name = Name, Description = Description };
    }
}
