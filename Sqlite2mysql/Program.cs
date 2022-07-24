// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Text.Json;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

if (environmentName == null)
    environmentName = "Development";

var builder = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{environmentName}.json")
        .AddEnvironmentVariables();
var configuration = builder.Build();
var sourceSqliteConnectionString = configuration.GetConnectionString("SQLiteConnection");
var targetMySqlConnectionString = configuration.GetConnectionString("MySQLConnection");
var targetMySqlVersion = new MySqlServerVersion(new Version(8, 0, 28));

var sqlitecontext = new Sqlite2mysql.SqliteBikeparkContext(sourceSqliteConnectionString);
sqlitecontext.Database.Migrate();
var mysqlcontext = new Sqlite2mysql.MySqlBikeparkContext(targetMySqlConnectionString, targetMySqlVersion);
mysqlcontext.Database.EnsureCreated();

log("SRC: sqlite ", sqlitecontext);

log("TRG: mysql  ", mysqlcontext);

Console.WriteLine("Clear TARGET MySQL DB Tables");

    mysqlcontext.UserRoles.RemoveRange(mysqlcontext.UserRoles);
    mysqlcontext.RoleClaims.RemoveRange(mysqlcontext.RoleClaims);
    mysqlcontext.Roles.RemoveRange(mysqlcontext.Roles);
    mysqlcontext.UserClaims.RemoveRange(mysqlcontext.UserClaims);
    mysqlcontext.UserLogins.RemoveRange(mysqlcontext.UserLogins);
    mysqlcontext.UserTokens.RemoveRange(mysqlcontext.UserTokens);
    mysqlcontext.Users.RemoveRange(mysqlcontext.Users);

    mysqlcontext.Holidays.RemoveRange(mysqlcontext.Holidays);
    mysqlcontext.ItemCategories.RemoveRange(mysqlcontext.ItemCategories);
    mysqlcontext.PricingCategories.RemoveRange(mysqlcontext.PricingCategories);
    mysqlcontext.ItemTypes.RemoveRange(mysqlcontext.ItemTypes.IgnoreQueryFilters());
    mysqlcontext.Pricings.RemoveRange(mysqlcontext.Pricings.IgnoreQueryFilters());
    mysqlcontext.Items.RemoveRange(mysqlcontext.Items.IgnoreQueryFilters());
    mysqlcontext.Prepared.RemoveRange(mysqlcontext.Prepared);
    mysqlcontext.Customers.RemoveRange(mysqlcontext.Customers);
    mysqlcontext.Records.RemoveRange(mysqlcontext.Records);
    mysqlcontext.ItemRecords.RemoveRange(mysqlcontext.ItemRecords);
await mysqlcontext.SaveChangesAsync();
log("TRG: mysql  ", mysqlcontext);

Console.WriteLine("Moving DB Tables from SOURCE SQLite to TARGET MySQL ");

foreach (var entity in sqlitecontext.Users)
    mysqlcontext.Users.Add(entity.CloneObject());

foreach (var entity in sqlitecontext.UserClaims)
{
    if (mysqlcontext.Users.Find(entity.UserId) != null)
        mysqlcontext.UserClaims.Add(entity.CloneObject());
}
foreach (var entity in sqlitecontext.UserLogins)
{
    if (mysqlcontext.Users.Find(entity.UserId) != null)
        mysqlcontext.UserLogins.Add(entity.CloneObject());
}
foreach (var entity in sqlitecontext.UserTokens)
{
    if (mysqlcontext.Users.Find(entity.UserId)!=null)
        mysqlcontext.UserTokens.Add(entity.CloneObject());
}
foreach (var entity in sqlitecontext.RoleClaims)
{
    if (mysqlcontext.Users.Find(entity.RoleId) != null)
        mysqlcontext.RoleClaims.Add(entity.CloneObject());
}
foreach (var entity in sqlitecontext.UserRoles)
{
    if (mysqlcontext.Users.Find(entity.RoleId) != null && mysqlcontext.Users.Find(entity.UserId) != null)
        mysqlcontext.UserRoles.Add(entity.CloneObject());
}

foreach (var entity in sqlitecontext.Holidays)
    mysqlcontext.Holidays.Add(entity.CloneObject());

foreach (var entity in sqlitecontext.ItemCategories)
    mysqlcontext.ItemCategories.Add(entity.CloneObject());

foreach (var entity in sqlitecontext.PricingCategories)
    mysqlcontext.PricingCategories.Add(entity.CloneObject());

foreach (var entity in sqlitecontext.Customers)
    mysqlcontext.Customers.Add(entity.CloneObject());

foreach (var entity in sqlitecontext.ItemTypes.IgnoreQueryFilters())
{
    var clone = entity.CloneObject();
    clone.ItemCategory = null;
    clone.PricingCategory = null;
    mysqlcontext.ItemTypes.Add(clone);
}
foreach (var entity in sqlitecontext.Pricings.IgnoreQueryFilters())
{
    var clone = entity.CloneObject();
    clone.PricingCategory = null;
    mysqlcontext.Pricings.Add(clone);
}
foreach (var entity in sqlitecontext.Items.IgnoreQueryFilters())
{
    var clone = entity.CloneObject();
    clone.ItemType = null;
    mysqlcontext.Items.Add(clone);
}
foreach (var entity in sqlitecontext.Prepared)
{
    var clone = entity.CloneObject();
    clone.Item = null;
    mysqlcontext.Prepared.Add(clone);
}

foreach (var entity in sqlitecontext.ItemRecords)
{
    var clone = entity.CloneObject();
    clone.User = null;
    clone.Pricing = null;
    clone.Item = null;
    clone.Record = null;
    Console.WriteLine(JsonSerializer.Serialize(clone));
    mysqlcontext.ItemRecords.Add(clone);
}
foreach (var entity in sqlitecontext.Records)
{
    var clone = entity.CloneObject();
    clone.User = null;
    clone.Customer = null;
    clone.ItemRecords.Clear();
    mysqlcontext.Records.Add(clone);
}

await mysqlcontext.SaveChangesAsync();
log("TRG: mysql  ", mysqlcontext);

Console.WriteLine("Press any key");
Console.ReadKey();

static void log(string prefix, Bikepark.Data.BikeparkContext context)
{
    Console.WriteLine(prefix + "Records              Count: {0}", context.Records.Count());
    Console.WriteLine(prefix + "Customers            Count: {0}", context.Customers.Count());
    Console.WriteLine(prefix + "ItemRecords          Count: {0}", context.ItemRecords.Count());
    Console.WriteLine(prefix + "Prepared             Count: {0}", context.Prepared.Count());
    Console.WriteLine(prefix + "Items                Count: {0}", context.Items.IgnoreQueryFilters().Count());
    Console.WriteLine(prefix + "ItemTypes            Count: {0}", context.ItemTypes.IgnoreQueryFilters().Count());
    Console.WriteLine(prefix + "ItemCategories       Count: {0}", context.ItemCategories.Count());
    Console.WriteLine(prefix + "Pricings             Count: {0}", context.Pricings.IgnoreQueryFilters().Count());
    Console.WriteLine(prefix + "PricingCategories    Count: {0}", context.PricingCategories.Count());
    Console.WriteLine(prefix + "Holidays             Count: {0}", context.Holidays.Count());
    Console.WriteLine(prefix + "UserRoles        Count: {0}", context.UserRoles.Count());
    Console.WriteLine(prefix + "RoleClaims       Count: {0}", context.RoleClaims.Count());
    Console.WriteLine(prefix + "Roles            Count: {0}", context.Roles.Count());
    Console.WriteLine(prefix + "UserClaims       Count: {0}", context.UserClaims.Count());
    Console.WriteLine(prefix + "UserLogins       Count: {0}", context.UserLogins.Count());
    Console.WriteLine(prefix + "UserTokens       Count: {0}", context.UserTokens.Count());
    Console.WriteLine(prefix + "Users            Count: {0}", context.Users.Count());
}

//}

//await host.RunAsync();
static class Helper
{
    public static T CloneObject<T>(this T obj) where T : class
    {
        if (obj == null) return null;
        System.Reflection.MethodInfo inst = obj.GetType().GetMethod("MemberwiseClone",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        if (inst != null)
            return (T)inst.Invoke(obj, null);
        else
            return null;
    }
}