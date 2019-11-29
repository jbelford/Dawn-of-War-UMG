using DowUmg.Data.Entities;
using DowUmg.Interfaces;
using Microsoft.EntityFrameworkCore;
using Splat;

namespace DowUmg.Data
{
    public class DataContext : DbContext
    {
        private readonly IFilePathProvider appDataProvider;

        public DataContext(IFilePathProvider? appDataProvider = null)
        {
            this.appDataProvider = appDataProvider ?? Locator.Current.GetService<IFilePathProvider>();
        }

        public DbSet<DowMap> Maps { get; set; } = null!;
        public DbSet<DowMod> Mods { get; set; } = null!;
        public DbSet<GameRule> GameRules { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={this.appDataProvider.DataLocation}",
                x => x.MigrationsAssembly("DowUmg.Migrations"));
        }
    }
}
