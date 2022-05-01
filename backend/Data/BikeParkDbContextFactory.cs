using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Bikepark.Data
{
    public class BikeParkDbContextFactory: IDesignTimeDbContextFactory<BikeparkContext>
    {
        public BikeparkContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "./"))
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("SQLiteConnection");    

            var optionsBuilder = new DbContextOptionsBuilder<BikeparkContext>();    
            optionsBuilder.UseSqlite(connectionString);

            return new BikeparkContext(optionsBuilder.Options);
        }
    }
}