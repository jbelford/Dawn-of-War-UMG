using System;
using System.Collections.Generic;
using System.Linq;
using DowUmg.Constants;
using DowUmg.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DowUmg.Data
{
    public class ModsDataStore : IDisposable
    {
        private readonly ModsContext context = new();

        public ModsDataStore() { }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public IQueryable<DowMod> GetAll()
        {
            return this.context.Mods;
        }

        public IQueryable<DowMod> GetPlayableMods()
        {
            return this
                .context.Mods.Where(mod => mod.Playable)
                .Where(mod =>
                    mod.Data.Maps.Count > 0 || mod.Data.Rules.Count > 0 || mod.Data.Races.Count > 0
                );
        }

        public IEnumerable<DowMap> GetAddonMaps()
        {
            return context
                .Mods.Where(mod => !mod.IsVanilla)
                .Where(mod => mod.Data.ModFolder == "dxp2" || mod.Data.ModFolder == "w40k")
                .SelectMany(mod => mod.Data.Maps);
        }

        public void DropAll()
        {
            context.Data.RemoveRange(context.Data);
            context.SaveChanges();
        }

        public void Add(DowMod mod)
        {
            var existing = context.Mods.SingleOrDefault(existing =>
                mod.ModFile == existing.ModFile
            );

            context.ChangeTracker.TrackGraph(
                mod,
                node =>
                    node.Entry.State = !node.Entry.IsKeySet
                        ? EntityState.Added
                        : EntityState.Unchanged
            );

            context.SaveChanges();
        }

        public IEnumerable<DowRace> GetRaces(int modId)
        {
            DowMod mod = GetAll().Single(mod => mod.Id == modId);

            return mod
                .Dependencies.SelectMany(dep => dep.Data.Races)
                .Concat(mod.Data.Races)
                .GroupBy(r => r.Name)
                .ToDictionary(g => g.Key, g => g.Last())
                .Values;
        }

        public IEnumerable<DowMap> GetMaps(int modId)
        {
            DowMod mod = GetAll().Single(mod => mod.Id == modId);

            return mod
                .Dependencies.SelectMany(dep => dep.Data.Maps)
                .Concat(mod.Data.Maps)
                .GroupBy(r => r.Name)
                .ToDictionary(g => g.Key, g => g.Last())
                .Values;
        }

        public IEnumerable<GameRule> GetRules(int modId)
        {
            DowMod mod = GetAll().Single(mod => mod.Id == modId);

            return mod
                .Dependencies.SelectMany(dep => dep.Data.Rules)
                .Concat(mod.Data.Rules)
                .GroupBy(r => r.Name)
                .ToDictionary(g => g.Key, g => g.Last())
                .Values;
        }

        public DowMap GetDowMap()
        {
            return context.Maps.First();
        }
    }
}
