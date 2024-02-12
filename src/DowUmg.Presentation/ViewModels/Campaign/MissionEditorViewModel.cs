using System.Reactive;
using DowUmg.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class MissionEditorViewModel : RoutableReactiveObject
    {
        public MissionEditorViewModel(IScreen screen, CampaignMission mission)
            : base(screen, "missionEdit")
        {
            CancelCommand = HostScreen.Router.NavigateBack;

            Name = mission.Name;
            Description = mission.Description;

            SaveCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                mission = new CampaignMission(mission.Map)
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
