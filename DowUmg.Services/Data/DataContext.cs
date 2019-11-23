using DowUmg.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Splat;

namespace DowUmg.Services.Data
{
    public class DataContext : DbContext
    {
        private IAppDataProvider appDataProvider;

        public DataContext(IAppDataProvider appDataProvider = null)
        {
            this.appDataProvider = appDataProvider ?? Locator.Current.GetService<IAppDataProvider>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={this.appDataProvider.DataLocation}");
        }
    }
}
