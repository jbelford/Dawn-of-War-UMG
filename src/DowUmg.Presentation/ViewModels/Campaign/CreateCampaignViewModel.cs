using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DowUmg.Models;
using DowUmg.Services;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class CreateCampaignViewModel : RoutableReactiveObject
    {
        private Campaign campaign = new();
        private SourceList<CampaignMission> missions = new();

        private readonly ICampaignService campaignService;
        private readonly CampaignMap defaultCampaignMap;

        public CreateCampaignViewModel(IScreen screen)
            : base(screen, "createcampaign")
        {
            campaignService = Locator.Current.GetService<ICampaignService>()!;
            defaultCampaignMap = campaignService.GetDefaultCampaignMap();

            AddMissionCommand = ReactiveCommand.CreateFromObservable(AddMission);

            this.WhenAnyValue(x => x.CampaignName, x => x.CampaignDescription)
                .Select(value => IsModified(value.Item1, value.Item2))
                .ToPropertyEx(this, x => x.IsChanged);

            missions
                .Connect()
                .Transform(
                    (mission, idx) =>
                        new MapListItemViewModel()
                        {
                            MapImage = mission.Map.ImagePath,
                            Header = $"Mission #{idx + 1}: {mission.Name}",
                            Details = mission.Description,
                            Footer = $"Map: {mission.Map.Name}"
                        }
                )
                .Bind(out _missionList)
                .Subscribe();
        }

        #region ViewModelProps

        [Reactive]
        public string CampaignName { get; set; }

        [Reactive]
        public string CampaignDescription { get; set; }

        [ObservableAsProperty]
        public bool IsChanged { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> AddMissionCommand { get; set; }

        private readonly ReadOnlyObservableCollection<MapListItemViewModel> _missionList;
        public ReadOnlyObservableCollection<MapListItemViewModel> MissionList => _missionList;

        #endregion

        private bool IsModified(string name, string desc) =>
            campaign.Name != name || campaign.Description != desc;

        private IObservable<IRoutableViewModel> AddMission()
        {
            var mission = new CampaignMission(defaultCampaignMap);
            missions.Add(mission);

            return HostScreen.Router.Navigate.Execute(
                new MissionEditorViewModel(HostScreen, mission, OnSaveNewMission)
            );
        }

        private void OnSaveNewMission(CampaignMission newMission)
        {
            missions.Edit(inner =>
            {
                inner.Remove(inner.Last());
                inner.Add(newMission);
            });
        }
    }
}
