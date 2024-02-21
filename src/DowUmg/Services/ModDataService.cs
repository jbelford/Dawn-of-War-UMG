using System.Collections.Generic;
using System.Linq;
using DowUmg.Data;
using DowUmg.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DowUmg.Services
{
    public interface IModDataService
    {
        public List<DowMod> GetMods();
        public List<DowMod> GetPlayableMods();
        public List<DowMap> GetAddonMaps();

        public void Add(IEnumerable<DowMod> mods);

        public void DropModData();

        public List<DowMap> GetModMaps(int modId);
        public List<GameRule> GetModRules(int modId);

        public DowMap GetDefaultMap();
    }

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

        public void DropModData()
        {
            using var context = new ModsContext();
            context.Data.RemoveRange(context.Data);
            context.SaveChanges();
        }

        public List<DowMap> GetAddonMaps()
        {
            using var context = new ModsContext();
            return context
                .Mods.Where(mod => !mod.IsVanilla)
                .Where(mod => mod.Data.ModFolder == "dxp2" || mod.Data.ModFolder == "w40k")
                .SelectMany(mod => mod.Data.Maps)
                .Include(map => map.Mod)
                .ToList();
        }

        public DowMap GetDefaultMap()
        {
            using var context = new ModsContext();
            return context.Maps.Include(map => map.Mod).First();
        }

        public List<DowMap> GetModMaps(int modId)
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
                .ToList();
        }

        public List<GameRule> GetModRules(int modId)
        {
            using var context = new ModsContext();
            return context
                .GameRules.FromSql(
                    $"""
                    select r.*
                    from GameRules r join (
                        select m.ModDataId
                        from DowModDowMod dm join Mods m on dm.DependenciesId = m.Id or m.Id = {modId}
                        where dm.DependentsId = {modId}
                    ) dep on r.ModDataId = dep.ModDataId
                    group by Name
                    """
                )
                .ToList();
        }

        public List<DowMod> GetMods()
        {
            using var context = new ModsContext();
            return context.Mods.ToList();
        }

        public List<DowMod> GetPlayableMods()
        {
            using var context = new ModsContext();
            return context
                .Mods.Where(mod => mod.Playable)
                .Where(mod =>
                    mod.Data.Maps.Count > 0 || mod.Data.Rules.Count > 0 || mod.Data.Races.Count > 0
                )
                .ToList();
        }
    }
}
