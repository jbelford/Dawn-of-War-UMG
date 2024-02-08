using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DowUmg.Data;
using DowUmg.Data.Entities;
using DowUmg.Models;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class CreateCampaignViewModel : RoutableReactiveObject
    {
        private Campaign campaign = new();

        public CreateCampaignViewModel(IScreen screen)
            : base(screen, "createcampaign")
        {
            using var modStore = new ModsDataStore();

            var defaultMap = modStore.GetDowMap();

            AddMissionCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                var mission = new MissionListItemViewModel(new CampaignMission(defaultMap));
                MissionList.Add(mission);

                return HostScreen.Router.Navigate.Execute(
                    new MissionEditorViewModel(HostScreen, mission)
                );
            });

            this.WhenAnyValue(x => x.CampaignName, x => x.CampaignDescription)
                .Select((_) => IsModified())
                .ToProperty(this, x => x.IsChanged, out _isChanged);
        }

        [Reactive]
        public string CampaignName { get; set; }

        [Reactive]
        public string CampaignDescription { get; set; }

        private readonly ObservableAsPropertyHelper<bool> _isChanged;
        public bool IsChanged => _isChanged.Value;

        public ReactiveCommand<Unit, IRoutableViewModel> AddMissionCommand { get; set; }

        public IObservableCollection<MissionListItemViewModel> MissionList { get; set; } =
            new ObservableCollectionExtended<MissionListItemViewModel>();

        private bool IsModified() =>
            campaign.Name != CampaignName || campaign.Description != CampaignDescription;
    }
}
