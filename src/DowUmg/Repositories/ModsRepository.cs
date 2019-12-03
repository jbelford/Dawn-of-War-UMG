using DowUmg.Data;
using DowUmg.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DowUmg.Repositories
{
    public class ModsRepository
    {
        public List<DowMod> GetAll()
        {
            using var context = new DataContext();
            return context.Mods.ToList();
        }

        public DowMod Upsert(DowMod mod)
        {
            using var context = new DataContext();
            using var transaction = context.Database.BeginTransaction();

            DowMod? existing = context.Mods.Find(mod.ModFolder);
            if (existing != null)
            {
                context.Mods.Remove(existing);
                context.SaveChanges();
            }

            context.Mods.Add(mod);
            context.SaveChanges();

            transaction.Commit();
            return mod;
        }
    }
}
