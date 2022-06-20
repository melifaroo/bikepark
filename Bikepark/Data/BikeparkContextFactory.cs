using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Bikepark.Data
{
    public class BikeparkContextFactory: IDesignTimeDbContextFactory<BikeparkContext>
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
            //optionsBuilder.UseMySql(  "server=localhost;database=bikepark;user=root;password=Passw0rd!", new MySqlServerVersion(new Version(8, 0, 28)) );

            return new BikeparkContext(optionsBuilder.Options);
        }
    }
}