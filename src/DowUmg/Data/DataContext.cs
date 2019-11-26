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

        public DbSet<ManyMatchupCampaign> Campaigns { get; set; } = null!;
        public DbSet<SingleMatchupCampaign> Matchups { get; set; } = null!;
        public DbSet<DowMap> Maps { get; set; } = null!;
        public DbSet<DowMod> Mods { get; set; } = null!;
        public DbSet<GameRule> GameRules { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={this.appDataProvider.DataLocation}",
                x => x.MigrationsAssembly("DowUmg.Migrations"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SingleMatchupCampaign>().HasBaseType<Campaign>();
            modelBuilder.Entity<ManyMatchupCampaign>().HasBaseType<Campaign>();

            modelBuilder.Entity<ScenarioPlayers>()
                .HasKey(sp => new { sp.ArmyId, sp.ScenarioId });
            modelBuilder.Entity<ScenarioPlayers>()
                .HasOne(sp => sp.Scenario)
                .WithMany(s => s.Players)
                .HasForeignKey(sp => sp.ScenarioId);
            modelBuilder.Entity<ScenarioPlayers>()
                .HasOne(sp => sp.Army)
                .WithMany(a => a.Scenarios)
                .HasForeignKey(sp => sp.ArmyId);

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
        }
    }
}
