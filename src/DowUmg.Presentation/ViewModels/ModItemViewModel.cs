﻿using DowUmg.FileFormats;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DowUmg.Presentation.ViewModels
{
    public class ModItemViewModel : ReactiveObject
    {
        [Reactive]
        public DowModuleFile Module { get; set; }

        public bool IsLoaded { get; set; }
    }
}