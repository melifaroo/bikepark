using Bikepark.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqlite2mysql
{
    public class SqliteBikeparkContext : Bikepark.Data.BikeparkContext
    {
        public SqliteBikeparkContext(string connectionString) : base( new DbContextOptionsBuilder<BikeparkContext>().UseSqlite(connectionString).Options )
        {
        }
    }
}
