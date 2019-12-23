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
                .Include(mod => mod.Dependencies)
                .Select(mod => new DowMod()
                {
                    Details = mod.Details,
                    IsVanilla = mod.IsVanilla,
                    ModFolder = mod.ModFolder,
                    Name = mod.Name,
                    MapsCount = mod.Maps.Count,
                    RacesCount = mod.Races.Count,
                    RulesCount = mod.Rules.Count
                }).ToList();
        }

        public DowMod Load(DowMod mod)
        {
            using var context = new DataContext();
            return context.Mods.Include(x => x.Maps)
                .Include(x => x.Rules)
                .Include(x => x.Races)
                .First(x => x.IsVanilla == mod.IsVanilla && string.Equals(x.ModFolder, mod.ModFolder));
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

            List<DowModDependency> dependencies = mod.Dependencies;

            mod.Dependencies = null;

            context.Mods.Add(mod);
            context.SaveChanges();

            foreach (var dep in dependencies)
            {
                dep.MainMod = mod;
                context.ChangeTracker.TrackGraph(dep, node =>
                    node.Entry.State = !node.Entry.IsKeySet ? EntityState.Added : EntityState.Unchanged);
                context.SaveChanges();
            }

            transaction.Commit();
            return mod;
        }

        public DowMod? Find(bool isVanilla, string modFolder)
        {
            using var context = new DataContext();
            return context.Mods.Where(existing => existing.IsVanilla == isVanilla &&
                    existing.ModFolder.Equals(modFolder, System.StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        public void DropAll()
        {
            using var context = new DataContext();
            context.Mods.RemoveRange(context.Mods);
            context.SaveChanges();
        }
    }
}
