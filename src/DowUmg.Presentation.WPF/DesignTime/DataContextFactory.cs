using DowUmg.Data;
using DowUmg.Presentation.WPF.Services;
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
