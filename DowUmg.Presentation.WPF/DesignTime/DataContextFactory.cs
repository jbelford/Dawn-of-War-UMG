using DowUmg.Presentation.WPF.Services;
using DowUmg.Services.Data;
using Microsoft.EntityFrameworkCore.Design;

namespace DowUmg.Presentation.DesignTime
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var filePathProvider = new WindowsFilePathProvider();
            return new DataContext(filePathProvider);
        }
    }
}
