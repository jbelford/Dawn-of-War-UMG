using DowUmg.Data;
using DowUmg.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DowUmg.Repositories
{
    internal class ModsRepository
    {
        public List<DowMod> GetAll()
        {
            using var context = new DataContext();
            return context.Mods
                .Select(mod => new DowMod()
                {
                    Details = mod.Details,
                    IsVanilla = mod.IsVanilla,
                    ModFolder = mod.ModFolder,
                    Name = mod.Name,
                    MapsCount = mod.Maps.Count,
                    RacesCount = mod.Races.Count,
                    RulesCount = mod.Rules.Count,
                    DependencyCount = mod.Dependencies.Count
                }).ToList();
        }

        public DowMod Upsert(DowMod mod)
        {
            using var context = new DataContext();
            using var transaction = context.Database.BeginTransaction();

            DowMod? existing = Find(mod.IsVanilla, mod.ModFolder);
            if (existing != null)
            {
                context.Mods.Remove(existing);
                context.SaveChanges();
            }

            context.ChangeTracker.TrackGraph(mod, node =>
                node.Entry.State = !node.Entry.IsKeySet ? EntityState.Added : EntityState.Unchanged);
            context.SaveChanges();

            transaction.Commit();
            return mod;
        }

        public DowMod? Find(bool isVanilla, string modFolder)
        {
            using var context = new DataContext();
            return context.Mods.SingleOrDefault(existing => existing.IsVanilla == isVanilla &&
                    existing.ModFolder.Equals(modFolder, System.StringComparison.OrdinalIgnoreCase));
        }

        public void DropAll()
        {
            using var context = new DataContext();
            context.Mods.RemoveRange(context.Mods);
            context.SaveChanges();
        }
    }
}
