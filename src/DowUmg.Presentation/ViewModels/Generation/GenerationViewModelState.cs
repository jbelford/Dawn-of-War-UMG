using System;
using System.Collections.Generic;
using System.Linq;
using DowUmg.Data.Entities;
using DowUmg.Services;
using DynamicData;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationViewModelState
    {
        private readonly IModDataService modDataService;

        private List<DowMap> _addonMaps;
        private List<DowMap> _maps;
        private SourceList<DowMap> _allowedAddonMaps = new();
        private SourceList<DowMap> _allowedMaps = new();
        private SourceList<GameRule> _rules = new();
        private Dictionary<int, bool> _allowedPlayers = new();
        private Dictionary<int, bool> _allowedSizes = new();

        public GenerationViewModelState(List<DowMap> addonMaps)
        {
            modDataService = Locator.Current.GetService<IModDataService>()!;
            _addonMaps = addonMaps;
        }

        public void RefreshForMod(int modId)
        {
            _maps = modDataService.GetModMaps(modId);
            _rules.Edit(inner =>
            {
                inner.Clear();
                inner.AddRange(modDataService.GetModRules(modId));
            });
            RefreshFilters();
        }

        public IObservable<IChangeSet<DowMap>> ConnectMainMaps() => _allowedMaps.Connect();

        public IObservable<IChangeSet<DowMap>> ConnectAddonMaps() => _allowedAddonMaps.Connect();

        public IObservable<IChangeSet<GameRule>> ConnectRules() => _rules.Connect();

        public void SetPlayersAllowed(int players, bool allowed)
        {
            if (_allowedPlayers.GetValueOrDefault(players, true) == allowed)
                return;

            _allowedPlayers[players] = allowed;
            RefreshFilters();
        }

        public void SetSizeAllowed(int size, bool allowed)
        {
            if (_allowedSizes.GetValueOrDefault(size, true) == allowed)
                return;

            _allowedSizes[size] = allowed;
            RefreshFilters();
        }

        private void RefreshFilters()
        {
            _allowedAddonMaps.Edit(inner =>
            {
                inner.Clear();
                inner.AddRange(
                    _addonMaps
                        .Where(map => _allowedPlayers.GetValueOrDefault(map.Players, true))
                        .Where(map => _allowedSizes.GetValueOrDefault(map.Size, true))
                );
            });
            _allowedMaps.Edit(inner =>
            {
                inner.Clear();
                inner.AddRange(
                    _maps
                        .Where(map => _allowedPlayers.GetValueOrDefault(map.Players, true))
                        .Where(map => _allowedSizes.GetValueOrDefault(map.Size, true))
                );
            });
        }
    }
}
