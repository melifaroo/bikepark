using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bikepark.Models;
using Bikepark.Authorization;

namespace Bikepark.Data
{
    public class BikeparkDbContext : IdentityDbContext
    {
        public DbSet<Item> Storage { get; set; }
        public DbSet<RentalRecord> RentalLog { get; set; }
        public DbSet<ServiceRecord> ServiceLog { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemSubCategory> ItemSubCategories { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BikeparkDbContext(DbContextOptions<BikeparkDbContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw="")
        {
            using (var context = new BikeparkDbContext( serviceProvider.GetRequiredService<DbContextOptions<BikeparkDbContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@bikepark.ru");
                await EnsureRole(serviceProvider, adminID, Constants.ContactAdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@bikepark.ru");
                await EnsureRole(serviceProvider, managerID, Constants.ContactManagersRole);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            if (userManager == null)
            {
            throw new Exception("userManager is null");
            }

            var user = await userManager.FindByNameAsync(UserName);
            
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            if (userManager == null)
            {
            throw new Exception("userManager is null");
            }

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Item>()
                .HasIndex(p => p.ItemID) // не Null
                .IsUnique();

            ItemCategory Bike, Scooter, Accessories;
            ItemSubCategory MTB, BMX, eBike, BalanceBike, eScooter, KickScooter, Helmet, Paddings, Gloves, Cam;

            modelBuilder.Entity<ItemSubCategory>().HasData(MTB = new ItemSubCategory { ItemSubCategoryID = 1, Name = "MTB", ItemCategoryID = 1 });

            modelBuilder.Entity<ItemSubCategory>().HasData(BMX = new ItemSubCategory { ItemSubCategoryID = 2, Name = "BMX", ItemCategoryID = 1 });

            modelBuilder.Entity<ItemSubCategory>().HasData(eBike = new ItemSubCategory { ItemSubCategoryID = 3, Name = "eBike", ItemCategoryID = 1 });

            modelBuilder.Entity<ItemSubCategory>().HasData(BalanceBike = new ItemSubCategory { ItemSubCategoryID = 4, Name = "BalanceBike", ItemCategoryID = 1 });

            modelBuilder.Entity<ItemSubCategory>().HasData(eScooter = new ItemSubCategory { ItemSubCategoryID = 5, Name = "eScooter", ItemCategoryID = 2 });

            modelBuilder.Entity<ItemSubCategory>().HasData(KickScooter = new ItemSubCategory { ItemSubCategoryID = 6, Name = "KickScooter", ItemCategoryID = 2 });

            modelBuilder.Entity<ItemSubCategory>().HasData(Helmet = new ItemSubCategory { ItemSubCategoryID = 7, Name = "Helmet", ItemCategoryID = 3 });

            modelBuilder.Entity<ItemSubCategory>().HasData(Paddings = new ItemSubCategory { ItemSubCategoryID = 8, Name = "Paddings", ItemCategoryID = 3 });

            modelBuilder.Entity<ItemSubCategory>().HasData(Gloves = new ItemSubCategory { ItemSubCategoryID = 9, Name = "Gloves", ItemCategoryID = 3 });

            modelBuilder.Entity<ItemSubCategory>().HasData(Cam = new ItemSubCategory { ItemSubCategoryID = 10, Name = "Cam", ItemCategoryID = 3 });

            modelBuilder.Entity<ItemCategory>().HasData(Bike = new ItemCategory { ItemCategoryID = 1, Name = "Bike" });

            modelBuilder.Entity<ItemCategory>().HasData(Scooter = new ItemCategory { ItemCategoryID = 2, Name = "Scooter" });

            modelBuilder.Entity<ItemCategory>().HasData(Accessories = new ItemCategory { ItemCategoryID = 3, Name = "Accessories" });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 1, ItemNumber = "1", ItemCategoryID = 1, ItemSubCategoryID = 1, ItemName = "Mongoose", ItemSize = "M", ItemStatus = ItemStatus.Ready });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 2, ItemNumber = "2", ItemCategoryID = 1, ItemSubCategoryID = 1, ItemName = "Mongoose", ItemSize = "M", ItemStatus = ItemStatus.Ready });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 3, ItemNumber = "3", ItemCategoryID = 1, ItemSubCategoryID = 1, ItemName = "Mongoose", ItemSize = "L", ItemStatus = ItemStatus.Ready });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 4, ItemNumber = "4", ItemCategoryID = 1, ItemSubCategoryID = 1, ItemName = "GT", ItemSize = "M", ItemStatus = ItemStatus.Ready });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 5, ItemNumber = "5", ItemCategoryID = 1, ItemSubCategoryID = 1, ItemName = "GT", ItemSize = "L", ItemStatus = ItemStatus.Ready });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 6, ItemNumber = "1", ItemCategoryID = 1, ItemSubCategoryID = 2, ItemName = "GT", ItemStatus = ItemStatus.Ready });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 7, ItemNumber = "2", ItemCategoryID = 1, ItemSubCategoryID = 2, ItemName = "GT", ItemStatus = ItemStatus.Ready });
        }
        
    }
}