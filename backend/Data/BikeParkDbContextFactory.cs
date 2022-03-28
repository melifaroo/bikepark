using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Bikepark.Data
{
    public class BikeParkDbContextFactory: IDesignTimeDbContextFactory<BikeparkDbContext>
    {
        public BikeparkDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "./"))
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("SQLiteConnection");    

            var optionsBuilder = new DbContextOptionsBuilder<BikeparkDbContext>();    
            optionsBuilder.UseSqlite(connectionString);

            return new BikeparkDbContext(optionsBuilder.Options);
        }
    }
}