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

        public DbSet<Campaign> Campaigns { get; set; } = null!;
        public DbSet<DowMap> Maps { get; set; } = null!;
        public DbSet<DowMod> Mods { get; set; } = null!;
        public DbSet<GameRule> GameRules { get; set; } = null!;
        public DbSet<SaveGame> SaveGames { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={this.appDataProvider.DataLocation}",
                x => x.MigrationsAssembly("DowUmg.Migrations"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Scenario>()
                .HasOne(s => s.Map)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ScenarioPlayers>()
                .HasKey(sp => new { sp.ArmyId, sp.ScenarioId });
            modelBuilder.Entity<ScenarioPlayers>()
                .HasOne(sp => sp.Scenario)
                .WithMany(s => s.Players)
                .HasForeignKey(sp => sp.ScenarioId);
            modelBuilder.Entity<ScenarioPlayers>()
                .HasOne(sp => sp.Army)
                .WithMany()
                .HasForeignKey(sp => sp.ArmyId);

            modelBuilder.Entity<Alliance>()
                .HasMany(al => al.Armies)
                .WithOne(a => a.Alliance!)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<GameInfoRule>()
                .HasKey(ir => new { ir.InfoId, ir.RuleId });
            modelBuilder.Entity<GameInfoRule>()
                .HasOne(ir => ir.Info)
                .WithMany(i => i.Rules)
                .HasForeignKey(ir => ir.InfoId);
            modelBuilder.Entity<GameInfoRule>()
                .HasOne(ir => ir.Rule)
                .WithMany(r => r.Infos)
                .HasForeignKey(ir => ir.RuleId);

            modelBuilder.Entity<SaveGame>()
                .HasOne(sg => sg.Scenario)
                .WithMany(s => s.SaveGames)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
