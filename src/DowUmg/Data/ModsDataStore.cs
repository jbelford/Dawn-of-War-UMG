using DowUmg.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void DropAll()
        {
            this.context.Mods.RemoveRange(this.context.Mods);
        }

        public void Add(DowMod mod)
        {
            var existing = context.Mods.SingleOrDefault(existing => existing.IsVanilla == mod.IsVanilla
                && existing.ModFolder.Equals(mod.ModFolder, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                context.Mods.Remove(existing);
                context.SaveChanges();
            }

            context.ChangeTracker.TrackGraph(mod, node =>
                node.Entry.State = !node.Entry.IsKeySet ? EntityState.Added : EntityState.Unchanged);

            context.SaveChanges();
        }

        public IEnumerable<DowRace> GetRaces(int modId)
        {
            DowMod mod = GetAll().Single(mod => mod.Id == modId);

            return mod.Dependencies.SelectMany(dep => dep.DepMod.Races).Concat(mod.Races)
                .GroupBy(r => r.Name)
                .ToDictionary(g => g.Key, g => g.Last())
                .Values;
        }

        public IEnumerable<DowMap> GetMaps(int modId)
        {
            DowMod mod = GetAll().Single(mod => mod.Id == modId);

            return mod.Dependencies.SelectMany(dep => dep.DepMod.Maps).Concat(mod.Maps)
                .GroupBy(r => r.Name)
                .ToDictionary(g => g.Key, g => g.Last())
                .Values;
        }

        public IEnumerable<GameRule> GetRules(int modId)
        {
            DowMod mod = GetAll().Single(mod => mod.Id == modId);

            return mod.Dependencies.SelectMany(dep => dep.DepMod.Rules).Concat(mod.Rules)
                .GroupBy(r => r.Name)
                .ToDictionary(g => g.Key, g => g.Last())
                .Values;
        }
    }
}
