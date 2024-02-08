using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace DowUmg.Presentation.ViewModels
{
    public class CampaignViewModel : RoutableReactiveObject
    {
        public CampaignViewModel(IScreen screen)
            : base(screen, "campaign")
        {
            CreateCampaignAction = ReactiveCommand.CreateFromObservable(
                () => HostScreen.Router.Navigate.Execute(new CreateCampaignViewModel(HostScreen))
            );
        }

        public ReactiveCommand<Unit, Unit> NewCampaignAction { get; }

        public ReactiveCommand<Unit, Unit> LoadCampaignAction { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> CreateCampaignAction { get; }
    }
}
