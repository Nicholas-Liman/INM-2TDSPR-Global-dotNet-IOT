using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AshBoard.Data.AppData
{
    public class AshBoardDbContextFactory : IDesignTimeDbContextFactory<AshBoardDbContext>
    {
        public AshBoardDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AshBoardDbContext>();

            optionsBuilder.UseOracle(configuration.GetConnectionString("DefaultConnection"));

            return new AshBoardDbContext(optionsBuilder.Options);
        }
    }
}
