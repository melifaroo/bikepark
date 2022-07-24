using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Bikepark.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("bikepark.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables(); ;

//var ConnectionString = builder.Configuration.GetConnectionString("SQLiteConnection");
var ConnectionString = builder.Configuration.GetConnectionString("MySQLConnection");
var MySQLServerVersion = builder.Configuration.GetValue<string>("MySQLServerVersion");

builder.Services.AddDbContext<BikeparkContext>(options => options
                                                .UseLazyLoadingProxies()
                                                //.UseSqlite(ConnectionString)
                                                .UseMySql(ConnectionString, new MySqlServerVersion(new Version(MySQLServerVersion)))
                                                ); 

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BikeparkContext>();

builder.Services.AddOptions();
builder.Services.Configure<Bikepark.BikeparkConfig>(builder.Configuration.GetSection("Bikepark"));


builder.Services.AddControllersWithViews(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var supportedCultures = new[] { "ru-RU", "ru" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);



var app = builder.Build();

app.UseRequestLocalization(localizationOptions);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var testUserPw = builder.Configuration.GetValue<string>("SeedUserPW");
    var rootPw = builder.Configuration.GetValue<string>("SeedRootPW");
    var db = scope.ServiceProvider.GetRequiredService<BikeparkContext>();
    db.Database.Migrate();
    await BikeparkContext.Initialize(services, testUserPw, rootPw);
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
