using DowUmg.Data.Entities;
using DowUmg.Interfaces;
using Microsoft.EntityFrameworkCore;
using Splat;

namespace DowUmg.Data
{
    public class ModsContext : DbContext
    {
        private readonly IFilePathProvider appDataProvider;

        public ModsContext(IFilePathProvider? appDataProvider = null)
        {
            this.appDataProvider =
                appDataProvider ?? Locator.Current.GetService<IFilePathProvider>();
        }

        public DbSet<DowMap> Maps { get; set; } = null!;
        public DbSet<DowMod> Mods { get; set; } = null!;
        public DbSet<GameRule> GameRules { get; set; } = null!;
        public DbSet<DowRace> Races { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite(
                    $"Data Source={this.appDataProvider.DataLocation}",
                    x => x.MigrationsAssembly("DowUmg.Migrations")
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DowMod>().HasIndex(x => new { x.IsVanilla, x.ModFolder });

            modelBuilder.Entity<DowMod>().HasMany(x => x.Dependencies).WithMany(x => x.Dependents);
            modelBuilder.Entity<DowMod>().HasMany(x => x.Dependents).WithMany(x => x.Dependencies);

            modelBuilder
                .Entity<DowMod>()
                .HasMany(x => x.Maps)
                .WithOne(x => x.Mod)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey("ModId");
            modelBuilder
                .Entity<DowMod>()
                .HasMany(x => x.Rules)
                .WithOne(x => x.Mod)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey("ModId");
            modelBuilder
                .Entity<DowMod>()
                .HasMany(x => x.Races)
                .WithOne(x => x.Mod)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey("ModId");
        }
    }
}
