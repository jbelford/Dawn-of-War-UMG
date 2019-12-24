using DowUmg.Data;
using DowUmg.Presentation.WPF.Services;
using Microsoft.EntityFrameworkCore.Design;

namespace DowUmg.Presentation.DesignTime
{
    internal class DataContextFactory : IDesignTimeDbContextFactory<ModsContext>
    {
        public ModsContext CreateDbContext(string[] args)
        {
            var filePathProvider = new WindowsFilePathProvider();
            return new ModsContext(filePathProvider);
        }
    }
}
