﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DowUmg.Data.Entities;
using DowUmg.Services;
using DynamicData;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    public class GenerationViewModelState
    {
        private readonly IModDataService modDataService;

        private List<DowMap> _addonMaps = new();
        private List<DowMap> _maps = new();
        private SourceList<DowMap> _allowedMaps = new();
        private SourceList<GameRule> _rules = new();
        private Dictionary<int, bool> _allowedPlayers = new();
        private Dictionary<int, bool> _allowedSizes = new();
        private bool _isAddonAllowed = true;
        private bool fetchedAddon = false;

        public GenerationViewModelState()
        {
            modDataService = Locator.Current.GetService<IModDataService>()!;
        }

        public async Task RefreshForMod(int modId)
        {
            if (!fetchedAddon)
            {
                _addonMaps = await modDataService.GetAddonMaps();
                fetchedAddon = true;
            }

            _maps = await modDataService.GetModMaps(modId);
            var rules = await modDataService.GetModRules(modId);
            _rules.Edit(inner =>
            {
                inner.Clear();
                inner.AddRange(rules);
            });
            RefreshFilters();
        }

        public IObservable<IChangeSet<DowMap>> ConnectMaps() => _allowedMaps.Connect();

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

        public bool IsAddonAllowed
        {
            get => _isAddonAllowed;
            set
            {
                _isAddonAllowed = value;
                RefreshFilters();
            }
        }

        private void RefreshFilters()
        {
            _allowedMaps.Edit(inner =>
            {
                inner.Clear();
                inner.AddRange(
                    (_isAddonAllowed ? _maps.Concat(_addonMaps) : _maps)
                        .Where(map => _allowedPlayers.GetValueOrDefault(map.Players, true))
                        .Where(map => _allowedSizes.GetValueOrDefault(map.Size, true))
                );
            });
        }
    }
}
