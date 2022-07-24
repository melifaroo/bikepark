using Bikepark.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite2mysql
{
    public class MySqlBikeparkContext : Bikepark.Data.BikeparkContext
    {
        public MySqlBikeparkContext(string connectionString, MySqlServerVersion version) 
            : base(new DbContextOptionsBuilder<BikeparkContext>()
                  .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 28)))
                  .EnableSensitiveDataLogging()
                  .Options )
        {
        }
    }
}
