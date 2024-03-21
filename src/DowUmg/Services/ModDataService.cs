using DowUmg.Data;
using DowUmg.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DowUmg.Services
{
    internal class ModDataService : IModDataService
    {
        public void Add(IEnumerable<DowMod> mods)
        {
            using var context = new ModsContext();

            foreach (var mod in mods)
            {
                context.ChangeTracker.TrackGraph(
                    mod,
                    node =>
                        node.Entry.State = !node.Entry.IsKeySet
                            ? EntityState.Added
                            : EntityState.Unchanged
                );
            }

            context.SaveChanges();
        }

        public void MigrateData()
        {
            using var context = new ModsContext();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
                context.Data.RemoveRange(context.Data);
                context.SaveChanges();
            }
        }

        public void DropModData()
        {
            using var context = new ModsContext();
            context.Data.RemoveRange(context.Data);
            context.SaveChanges();
        }

        public Task<List<DowMap>> GetAddonMaps()
        {
            using var context = new ModsContext();
            return context
                .Mods.Where(mod => !mod.IsVanilla)
                .Where(mod => mod.Data.ModFolder == "dxp2" || mod.Data.ModFolder == "w40k")
                .SelectMany(mod => mod.Data.Maps)
                .Include(map => map.Mod)
                .ToListAsync();
        }

        public DowMap GetDefaultMap()
        {
            using var context = new ModsContext();
            return context.Maps.Include(map => map.Mod).First();
        }

        public Task<List<DowMap>> GetModMaps(int modId)
        {
            using var context = new ModsContext();
            return context
                .Maps.FromSql(
                    $"""
                    select mp.*
                    from Maps mp join (
                        select m.ModDataId
                        from DowModDowMod dm join Mods m on dm.DependenciesId = m.Id or m.Id = {modId}
                        where dm.DependentsId = {modId}
                    ) dep on mp.ModDataId = dep.ModDataId
                    group by Name
                    """
                )
                .Include(map => map.Mod)
                .ToListAsync();
        }

        public Task<List<GameRule>> GetModRules(int modId)
        {
            using var context = new ModsContext();
            return context
                .GameRules.FromSql(
                    $"""
                    select *
                    from (
                        select r.Id, r.Name, r.Details, r.FileName, MAX(r.ModDataId) as ModDataId, r.IsWinCondition
                        from GameRules r join (
                            select m.ModDataId
                            from DowModDowMod dm join Mods m on dm.DependenciesId = m.Id or m.Id = {modId}
                            where dm.DependentsId = {modId}
                        ) dep on r.ModDataId = dep.ModDataId
                        group by r.FileName
                    )
                    group by Name
                    order by Name
                    """
                )
                .ToListAsync();
        }

        public Task<List<DowRace>> GetRaces(int modId)
        {
            using var context = new ModsContext();
            return context
                .Races.FromSql(
                    $"""
                    select r.Id, r.Name, r.Description, MAX(r.ModDataId) as ModDataId, r.FileName
                    from Races r join (
                        select m.ModDataId
                        from DowModDowMod dm join Mods m on dm.DependenciesId = m.Id or m.Id = {modId}
                        where dm.DependentsId = {modId}
                    ) dep on r.ModDataId = dep.ModDataId
                    group by r.FileName
                    order by r.Name
                    """
                )
                .ToListAsync();
        }

        public List<DowMod> GetMods()
        {
            using var context = new ModsContext();
            return context.Mods.ToList();
        }

        public Task<List<DowMod>> GetPlayableMods()
        {
            using var context = new ModsContext();
            return context
                .Mods.Where(mod => mod.Playable)
                .Where(mod =>
                    mod.Data.Maps.Count > 0 || mod.Data.Rules.Count > 0 || mod.Data.Races.Count > 0
                )
                .ToListAsync();
        }
    }
}
