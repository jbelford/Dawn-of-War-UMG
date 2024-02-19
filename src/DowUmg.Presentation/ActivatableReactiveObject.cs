using ReactiveUI;

namespace DowUmg.Presentation
{
    public class ActivatableReactiveObject : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        public ActivatableReactiveObject()
        {
            Activator = new ViewModelActivator();
        }
    }
}
