﻿using ReactiveUI;

namespace DowUmg.Presentation
{
    public class RoutableReactiveObject : ActivatableReactiveObject, IRoutableViewModel
    {
        public RoutableReactiveObject(IScreen screen, string urlPathSegment)
        {
            HostScreen = screen;
            UrlPathSegment = urlPathSegment;
        }

        public string UrlPathSegment { get; }

        public IScreen HostScreen { get; }
    }
}
