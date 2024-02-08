using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DowUmg.Data.Entities;
using DowUmg.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class MissionListItemViewModel : MapListItemViewModel
    {
        public MissionListItemViewModel(CampaignMission mission)
        {
            Mission = mission;

            this.WhenAnyValue(x => x.Mission).Subscribe(OnMissionUpdate);
        }

        [Reactive]
        public CampaignMission Mission { get; set; }

        private void OnMissionUpdate(CampaignMission mission)
        {
            MapImage = mission.Map.Image;
            Header = mission.Name;
            Details = mission.Description;
            Footer = $"Map: {mission.Map.Name}";
        }
    }
}
