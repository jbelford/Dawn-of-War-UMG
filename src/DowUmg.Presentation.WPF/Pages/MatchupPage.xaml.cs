using DowUmg.Presentation.ViewModels;
using ReactiveUI;

namespace DowUmg.Presentation.WPF.Pages
{
    /// <summary>
    /// Interaction logic for MatchupPage.xaml
    /// </summary>
    public partial class MatchupPage : ReactiveUserControl<MatchupViewModel>
    {
        public MatchupPage()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
            });
        }
    }
}
