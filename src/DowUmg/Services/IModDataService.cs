using System.Collections.Generic;
using System.Threading.Tasks;
using DowUmg.Data.Entities;

namespace DowUmg.Services
{
    public interface IModDataService
    {
        public List<DowMod> GetMods();
        public Task<List<DowMod>> GetPlayableMods();
        public Task<List<DowMap>> GetAddonMaps();

        public void Add(IEnumerable<DowMod> mods);

        public void MigrateData();

        public void DropModData();

        public Task<List<DowMap>> GetModMaps(int modId);
        public Task<List<GameRule>> GetModRules(int modId);
        public Task<List<DowRace>> GetRaces(int modId);

        public DowMap GetDefaultMap();
    }
}
