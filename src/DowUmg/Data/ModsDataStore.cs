using System;
using System.Collections.Generic;
using System.Linq;
using DowUmg.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DowUmg.Data
{
    public class ModsDataStore : IDisposable
    {
        private readonly ModsContext context;

        public ModsDataStore()
        {
            this.context = new ModsContext();
        }

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
                .Where(mod => mod.Maps.Count > 0 || mod.Rules.Count > 0 || mod.Races.Count > 0);
        }

        public void DropAll()
        {
            this.context.Mods.RemoveRange(this.context.Mods);
        }

        public void Add(DowMod mod)
        {
            var existing = context.Mods.SingleOrDefault(existing =>
                existing.IsVanilla == mod.IsVanilla && existing.ModFolder == mod.ModFolder
            );

            if (existing != null)
            {
                context.Mods.Remove(existing);
                context.SaveChanges();
            }

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
                .Dependencies.SelectMany(dep => dep.Races)
                .Concat(mod.Races)
                .GroupBy(r => r.Name)
                .ToDictionary(g => g.Key, g => g.Last())
                .Values;
        }

        public IEnumerable<DowMap> GetMaps(int modId)
        {
            DowMod mod = GetAll().Single(mod => mod.Id == modId);

            return mod
                .Dependencies.SelectMany(dep => dep.Maps)
                .Concat(mod.Maps)
                .GroupBy(r => r.Name)
                .ToDictionary(g => g.Key, g => g.Last())
                .Values;
        }

        public IEnumerable<GameRule> GetRules(int modId)
        {
            DowMod mod = GetAll().Single(mod => mod.Id == modId);

            return mod
                .Dependencies.SelectMany(dep => dep.Rules)
                .Concat(mod.Rules)
                .GroupBy(r => r.Name)
                .ToDictionary(g => g.Key, g => g.Last())
                .Values;
        }
    }
}
