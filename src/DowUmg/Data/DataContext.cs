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
        public DbSet<DowModDependency> ModDependencies { get; set; } = null!;
        public DbSet<GameRule> GameRules { get; set; } = null!;
        public DbSet<DowRace> Races { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={this.appDataProvider.DataLocation}",
                x => x.MigrationsAssembly("DowUmg.Migrations"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DowMod>().HasIndex(x => new { x.IsVanilla, x.ModFolder });

            modelBuilder.Entity<DowModDependency>().HasKey(x => new { x.MainModId, x.DepModId });
            modelBuilder.Entity<DowModDependency>().HasOne(x => x.MainMod)
                .WithMany(x => x.Dependencies)
                .HasForeignKey(x => x.MainModId);
            modelBuilder.Entity<DowModDependency>().HasOne(x => x.DepMod)
                .WithMany(x => x.Dependents)
                .HasForeignKey(x => x.DepModId);

            modelBuilder.Entity<DowMod>().HasMany(x => x.Maps)
                .WithOne(x => x.Mod)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey("ModId");
            modelBuilder.Entity<DowMod>().HasMany(x => x.Rules)
                .WithOne(x => x.Mod)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey("ModId");
            modelBuilder.Entity<DowMod>().HasMany(x => x.Races)
                .WithOne(x => x.Mod)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey("ModId");

            modelBuilder.Entity<DowMap>().Property<int>("Id");
            modelBuilder.Entity<GameRule>().Property<int>("Id");
            modelBuilder.Entity<DowRace>().Property<int>("Id");
        }
    }
}
