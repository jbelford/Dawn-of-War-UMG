using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DowUmg.Data.Entities;
using DowUmg.Services;
using DynamicData;
using Splat;

namespace DowUmg.Presentation.ViewModels
{
    /// <summary>
    /// Stores details about current selected mod. Allowing connecting and subscribing
    /// to these details when mod is changed.
    /// </summary>
    public class ModGenerationState
    {
        private readonly IModDataService modDataService;

        private int? currentModId;
        private List<DowMap> _addonMaps = new();
        private List<DowMap> _maps = new();
        private SourceList<DowMap> _allowedMaps = new();
        private SourceList<string> _tags = new();
        private SourceList<GameRule> _rules = new();
        private SourceList<DowRace> _races = new();
        private Dictionary<int, bool> _allowedPlayers = new();
        private Dictionary<int, bool> _allowedSizes = new();
        private Dictionary<string, bool> _allowedTags = new();
        private bool _isAddonAllowed = true;
        private bool fetchedAddon = false;

        public ModGenerationState()
        {
            modDataService = Locator.Current.GetService<IModDataService>()!;
        }

        public async Task RefreshForMod(int modId)
        {
            if (modId == currentModId)
            {
                return;
            }
            currentModId = modId;

            if (!fetchedAddon)
            {
                _addonMaps = await modDataService.GetAddonMaps();
                fetchedAddon = true;
            }

            _maps = await modDataService.GetModMaps(modId);

            _tags.Edit(inner =>
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                inner.Clear();
                inner.AddRange(
                    _maps
                        .Select(m => m.Tag)
                        .Where(tag => !string.IsNullOrEmpty(tag))
                        .ToHashSet()
                        .Select(tag => textInfo.ToTitleCase(tag!))
                        .Append("Default")
                );
            });

            var rules = await modDataService.GetModRules(modId);
            _rules.Edit(inner =>
            {
                inner.Clear();
                inner.AddRange(rules);
            });

            var races = await modDataService.GetRaces(modId);
            _races.Edit(inner =>
            {
                inner.Clear();
                inner.AddRange(races);
            });

            RefreshFilters();
        }

        public IObservable<IChangeSet<DowMap>> ConnectMaps() => _allowedMaps.Connect();

        public IObservable<IChangeSet<GameRule>> ConnectRules() => _rules.Connect();

        public IObservable<IChangeSet<DowRace>> ConnectRaces() => _races.Connect();

        public IObservable<IChangeSet<string>> ConnectTags() => _tags.Connect();

        public bool SetPlayersAllowed(int players, bool allowed)
        {
            if (_allowedPlayers.GetValueOrDefault(players, true) == allowed)
            {
                return false;
            }

            _allowedPlayers[players] = allowed;
            return true;
        }

        public void SetSizeAllowed(int size, bool allowed)
        {
            if (_allowedSizes.GetValueOrDefault(size, true) == allowed)
            {
                return;
            }

            _allowedSizes[size] = allowed;
            RefreshFilters();
        }

        public void SetTagAllowed(string tag, bool allowed)
        {
            if (_allowedTags.GetValueOrDefault(tag, tag == "default") == allowed)
            {
                return;
            }
            _allowedTags[tag] = allowed;
            RefreshFilters();
        }

        public bool IsAddonAllowed
        {
            get => _isAddonAllowed;
            set
            {
                if (_isAddonAllowed != value)
                {
                    _isAddonAllowed = value;
                    RefreshFilters();
                }
            }
        }

        public void RefreshFilters()
        {
            _allowedMaps.Edit(inner =>
            {
                inner.Clear();
                inner.AddRange(
                    (_isAddonAllowed ? _maps.Concat(_addonMaps) : _maps)
                        .Where(map => _allowedPlayers.GetValueOrDefault(map.Players, true))
                        .Where(map => _allowedSizes.GetValueOrDefault(map.Size, true))
                        .Where(map =>
                            _allowedTags.GetValueOrDefault(map.Tag ?? "default", map.Tag == null)
                        )
                );
            });
        }
    }
}
