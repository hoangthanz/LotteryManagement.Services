using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LotteryManagement.Data.EF
{
    public class LotteryManageContextFactory : IDesignTimeDbContextFactory<LotteryManageDbContext>
    {
        public LotteryManageDbContext CreateDbContext(string[] args)
        {
            /* Read current folder and get connection string in appsettings.json file */

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json")
                .Build();


            var connectionString = configuration.GetConnectionString("lotteryManagementDb");

            var optionsBuilder = new DbContextOptionsBuilder<LotteryManageDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LotteryManageDbContext(optionsBuilder.Options);
        }
    }
}
