using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bikepark.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;

namespace Bikepark.Data
{
    public class BikeparkContext : IdentityDbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemRecord> ItemRecords { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<PricingCategory> PricingCategories { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<ItemPrepared> Prepared { get; set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BikeparkContext(DbContextOptions<BikeparkContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw="", string rootPw="")
        {
            using (var context = new BikeparkContext( serviceProvider.GetRequiredService<DbContextOptions<BikeparkContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@bikepark.com");
                await EnsureRole(serviceProvider, adminID, BikeparkConfig.AdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@bikepark.com");
                await EnsureRole(serviceProvider, managerID, BikeparkConfig.ManagersRole);

                // allowed user can create and edit contacts that they create
                var rootID = await EnsureUser(serviceProvider, rootPw, "root@bikepark.com");
                await EnsureRole(serviceProvider, rootID, BikeparkConfig.ManagersRole);
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

            // modelBuilder.Entity<Pricing>()
            //     .Property(e => e.DaysOfWeek)
            //     .HasConversion(
            //           v => string.Join(",", v.Select(e => e.ToString()).ToArray()),
            //           v => v.Split(new[] { ',' })
            //             .Select(e => (DayOfWeek)(Enum.Parse(typeof(DayOfWeek), e)))
            //             .Cast<DayOfWeek>()
            //             .ToList()
            //     );
            modelBuilder.Entity<ItemType>().HasQueryFilter(m => EF.Property<bool>(m, "Archival") == false);
            modelBuilder.Entity<Item>().HasQueryFilter(m => EF.Property<bool>(m, "Archival") == false);
            modelBuilder.Entity<Pricing>().HasQueryFilter(m => EF.Property<bool>(m, "Archival") == false);

            DemoSeed(modelBuilder);
        }

        private static void DemoSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemCategory>().HasData(new ItemCategory { ItemCategoryID = 1, ItemCategoryName = "MTB" });
            modelBuilder.Entity<ItemCategory>().HasData(new ItemCategory { ItemCategoryID = 2, ItemCategoryName = "MTB Teenage" });
            modelBuilder.Entity<ItemCategory>().HasData(new ItemCategory { ItemCategoryID = 3, ItemCategoryName = "BMX" });
            modelBuilder.Entity<ItemCategory>().HasData(new ItemCategory { ItemCategoryID = 4, ItemCategoryName = "Balance" });
            modelBuilder.Entity<ItemCategory>().HasData(new ItemCategory { ItemCategoryID = 5, ItemCategoryName = "Electric" });
            modelBuilder.Entity<ItemCategory>().HasData(new ItemCategory { ItemCategoryID = 6, ItemCategoryName = "Accessory", Accessories = true });

            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 1, PricingCategoryName = "MTB" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 2, PricingCategoryName = "Teenage" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 3, PricingCategoryName = "BMX" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 4, PricingCategoryName = "Child" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 5, PricingCategoryName = "Electric" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 6, PricingCategoryName = "Accessory" });

            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 1, ItemCategoryID = 1, PricingCategoryID = null, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = "27''", ItemColor = "black", ItemDescription = "Mountain Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null }); ;
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 2, ItemCategoryID = null, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = "27''", ItemColor = "aqua", ItemDescription = "Mountain Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 3, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = "27''", ItemColor = "black", ItemDescription = "Mountain Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 4, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = "27''", ItemColor = "aqua", ItemDescription = "Mountain Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 5, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 29 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.XL, ItemWheelSize = "29''", ItemColor = "black", ItemDescription = "Mountain Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/2541638.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 6, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 29 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.XL, ItemWheelSize = "29''", ItemColor = "aqua", ItemDescription = "Mountain Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/2541638.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 7, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "Mongoose SWITCHBACK SPORT W", ItemAge = null, ItemGender = ItemGender.W, ItemSize = ItemSize.S, ItemWheelSize = "27.5''", ItemColor = "navy", ItemDescription = "Mountain Bike, Women's", ItemExternalURL = "https://trial-sport.ru/goods/51516/1496760.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 8, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "Mongoose SWITCHBACK SPORT W", ItemAge = null, ItemGender = ItemGender.W, ItemSize = ItemSize.M, ItemWheelSize = "27.5''", ItemColor = "navy", ItemDescription = "Mountain Bike, Women's", ItemExternalURL = "https://trial-sport.ru/goods/51516/1496760.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 9, ItemCategoryID = 2, PricingCategoryID = 2, ItemTypeName = "Mongoose ROCKADILE 20", ItemAge = ItemAge.Teen, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = "20''", ItemColor = "red", ItemDescription = "Mountain Bike, Teenage", ItemExternalURL = "https://trial-sport.ru/goods/51516/1493214.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 10, ItemCategoryID = 2, PricingCategoryID = 2, ItemTypeName = "Mongoose ROCKADILE 20 W", ItemAge = ItemAge.Teen, ItemGender = ItemGender.W, ItemSize = ItemSize.M, ItemWheelSize = "20''", ItemColor = "purple", ItemDescription = "Mountain Bike, Teenage", ItemExternalURL = "https://trial-sport.ru/goods/51516/1496770.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 11, ItemCategoryID = 3, PricingCategoryID = 3, ItemTypeName = "Radio SAIKO 20", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "black", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2033001.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 12, ItemCategoryID = 3, PricingCategoryID = 3, ItemTypeName = "Radio SAIKO 20", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "metallic purple", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2033001.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 13, ItemCategoryID = 3, PricingCategoryID = 3, ItemTypeName = "Radio DARKO", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "matt black", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2033004.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 14, ItemCategoryID = 3, PricingCategoryID = 3, ItemTypeName = "WeThePeople ARCADE", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "matt black", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2032941.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 15, ItemCategoryID = 4, PricingCategoryID = 4, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "yellow", ItemDescription = "Balance Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/1493326.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 16, ItemCategoryID = 4, PricingCategoryID = 4, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "black", ItemDescription = "Balance Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/1493326.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 17, ItemCategoryID = 4, PricingCategoryID = 4, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "orange", ItemDescription = "Balance Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/1411943.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 18, ItemCategoryID = 4, PricingCategoryID = 4, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "blue", ItemDescription = "Balance Bike", ItemExternalURL = "https://trial-sport.ru/goods/51516/1411943.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 19, ItemCategoryID = 5, PricingCategoryID = 5, ItemTypeName = "Himo C26", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "26''", ItemColor = null, ItemDescription = "E-Bike, Electric Bike", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 20, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Helmet", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Helmet", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 21, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Helmet", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Helmet", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 22, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Helmet", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Helmet", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 23, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Helmet", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Helmet, Children's", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 24, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Knee Pads", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Knee Pads", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 25, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Knee Pads", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Knee Pads", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 26, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Knee Pads", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Knee Pads", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 27, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Knee Pads", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Knee Pads, Children's", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 28, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Elbow Pads", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Elbow Pads", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 29, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Elbow Pads", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Elbow Pads", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 30, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Elbow Pads", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Elbow Pads", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 31, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Elbow Pads", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Elbow Pads, Children's", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 32, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Gloves", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Gloves", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 33, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Gloves", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Gloves", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 34, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Gloves", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Gloves", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 35, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Gloves", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Gloves, Children's", ItemExternalURL = "", ItemImageURL = null });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 1, ItemTypeID = 1, ItemNumber = "101" });//, ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 2, ItemTypeID = 1, ItemNumber = "102" });//, ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 3, ItemTypeID = 2, ItemNumber = "103" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 4, ItemTypeID = 2, ItemNumber = "104" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 5, ItemTypeID = 3, ItemNumber = "105" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 6, ItemTypeID = 3, ItemNumber = "106" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 7, ItemTypeID = 4, ItemNumber = "107" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 8, ItemTypeID = 4, ItemNumber = "108" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 9, ItemTypeID = 5, ItemNumber = "109" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 10, ItemTypeID = 5, ItemNumber = "110" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 11, ItemTypeID = 5, ItemNumber = "111" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 12, ItemTypeID = 6, ItemNumber = "112" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 13, ItemTypeID = 7, ItemNumber = "113" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 14, ItemTypeID = 8, ItemNumber = "114" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 15, ItemTypeID = 8, ItemNumber = "115" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 16, ItemTypeID = 9, ItemNumber = "116" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 17, ItemTypeID = 9, ItemNumber = "117" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 18, ItemTypeID = 10, ItemNumber = "118" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 19, ItemTypeID = 11, ItemNumber = "201" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 20, ItemTypeID = 12, ItemNumber = "202" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 21, ItemTypeID = 13, ItemNumber = "203" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 22, ItemTypeID = 13, ItemNumber = "204" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 23, ItemTypeID = 14, ItemNumber = "205" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 24, ItemTypeID = 14, ItemNumber = "206" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 25, ItemTypeID = 15, ItemNumber = "301" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 26, ItemTypeID = 16, ItemNumber = "302" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 27, ItemTypeID = 17, ItemNumber = "303" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 28, ItemTypeID = 18, ItemNumber = "304" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 29, ItemTypeID = 19, ItemNumber = "401" });//, ItemStatus = ItemStatus.RentedOut });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 30, ItemTypeID = 19, ItemNumber = "402" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 31, ItemTypeID = 19, ItemNumber = "403" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 32, ItemTypeID = 19, ItemNumber = "404" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 33, ItemTypeID = 19, ItemNumber = "405" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 34, ItemTypeID = 20, ItemNumber = "1" });//, ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 35, ItemTypeID = 21, ItemNumber = "2" });//, ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 36, ItemTypeID = 21, ItemNumber = "3" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 37, ItemTypeID = 22, ItemNumber = "4" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 38, ItemTypeID = 23, ItemNumber = "5" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 39, ItemTypeID = 23, ItemNumber = "6" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 40, ItemTypeID = 24, ItemNumber = "7" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 41, ItemTypeID = 25, ItemNumber = "8" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 42, ItemTypeID = 25, ItemNumber = "9" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 43, ItemTypeID = 26, ItemNumber = "10" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 44, ItemTypeID = 27, ItemNumber = "11" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 45, ItemTypeID = 27, ItemNumber = "12" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 46, ItemTypeID = 28, ItemNumber = "13" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 47, ItemTypeID = 29, ItemNumber = "14" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 48, ItemTypeID = 29, ItemNumber = "15" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 49, ItemTypeID = 30, ItemNumber = "16" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 50, ItemTypeID = 31, ItemNumber = "17" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 51, ItemTypeID = 31, ItemNumber = "18" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 52, ItemTypeID = 32, ItemNumber = "19" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 53, ItemTypeID = 33, ItemNumber = "20" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 54, ItemTypeID = 33, ItemNumber = "21" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 55, ItemTypeID = 34, ItemNumber = "22" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 56, ItemTypeID = 35, ItemNumber = "23" });//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 57, ItemTypeID = 35, ItemNumber = "24" });//, ItemStatus = ItemStatus.Available });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 1, PricingName = "MTB - weekday - 1 hour", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.WeekdayShort, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 2, PricingName = "MTB - friday - 1 hour", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.Friday, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 3, PricingName = "MTB - day-off - 1 hour", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.Weekend, IsHoliday = false, IsReduced = false, Price = 200 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 4, PricingName = "MTB - day-off - 1 hour", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = true, IsReduced = false, Price = 200 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 5, PricingName = "MTB - concessional - 1 hour", PricingCategoryID = null, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = false, IsReduced = true, Price = 100 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 28, PricingName = "MTB - weekday - 2 hours", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.WeekdayShort, IsHoliday = false, IsReduced = false, Price = 250 / 2, MinDuration = 2 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 29, PricingName = "MTB - friday - 2 hours", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.Friday, IsHoliday = false, IsReduced = false, Price = 250 / 2, MinDuration = 2 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 30, PricingName = "MTB - day-off - 2 hours", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.Weekend, IsHoliday = false, IsReduced = false, Price = 300 / 2, MinDuration = 2 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 31, PricingName = "MTB - holiday - 2 hours", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = true, IsReduced = false, Price = 300 / 2, MinDuration = 2 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 32, PricingName = "MTB - concessional - 2 hours", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = false, IsReduced = true, Price = 150 / 2, MinDuration = 2 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 6, PricingName = "MTB - weekday - whole day (1 day)", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = DaysOfWeekFlags.WeekdayShort, IsHoliday = false, IsReduced = false, Price = 800 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 7, PricingName = "MTB - friday - whole day (1 day)", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = DaysOfWeekFlags.Friday, IsHoliday = false, IsReduced = false, Price = 1000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 8, PricingName = "MTB - day-off - whole day (1 day)", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = DaysOfWeekFlags.Weekend, IsHoliday = false, IsReduced = false, Price = 1000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 9, PricingName = "MTB - holiday - whole day (1 day)", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = true, IsReduced = false, Price = 1000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 10, PricingName = "MTB - concessional - whole day (1 day)", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = false, IsReduced = true, Price = 400 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 11, PricingName = "MTB Teenager - weekday - 1 hour", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.WeekdayShort, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 12, PricingName = "MTB Teenager - friday - 1 hour", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.Friday, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 13, PricingName = "MTB Teenager - day-off - 1 hour", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.Weekend, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 14, PricingName = "MTB Teenager - holiday - 1 hour", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = true, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 15, PricingName = "MTB Teenager - concessional - 1 hour", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = false, IsReduced = true, Price = 50 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 16, PricingName = "MTB Teenager - weekday - whole day (1 day)", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.WeekdayShort, IsHoliday = false, IsReduced = false, Price = 500 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 17, PricingName = "MTB Teenager - friday - whole day (1 day)", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.Friday, IsHoliday = false, IsReduced = false, Price = 600 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 18, PricingName = "MTB Teenager - day-off - whole day (1 day)", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.Weekend, IsHoliday = false, IsReduced = false, Price = 700 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 19, PricingName = "MTB Teenager - holiday - whole day (1 day)", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = true, IsReduced = false, Price = 700 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 20, PricingName = "MTB Teenager - concessional - whole day (1 day)", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = false, IsReduced = true, Price = 300 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 21, PricingName = "BMX - weekday - 1 hour", PricingCategoryID = 3, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.WeekdayShort, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 22, PricingName = "BMX - DaysOfWeekFlags.Weekend+Fri - 1 hour", PricingCategoryID = 3, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.LongWeekend, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 23, PricingName = "BMX - holiday - 1 hour", PricingCategoryID = 3, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = true, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 24, PricingName = "Balance - 1 hour", PricingCategoryID = 4, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 25, PricingName = "Electro - 1 hour", PricingCategoryID = 5, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = false, IsReduced = false, Price = 300 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 26, PricingName = "Accessory - 1 hour", PricingCategoryID = 6, PricingType = PricingType.Hourly, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = false, IsReduced = false, Price = 50 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 27, PricingName = "Accessory - whole day (1 day)", PricingCategoryID = 6, PricingType = PricingType.OneTime, DaysOfWeek = DaysOfWeekFlags.AllDays, IsHoliday = false, IsReduced = false, Price = 300 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 33, PricingName = "Tire Repair", PricingCategoryID = null, PricingType = PricingType.Service, Price = 2000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 34, PricingName = "Chain Repair", PricingCategoryID = null, PricingType = PricingType.Service, Price = 2000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 35, PricingName = "Brake Repair", PricingCategoryID = null, PricingType = PricingType.Service, Price = 1000 });

            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerID = 1, CustomerFullName = "John Doe", CustomerDocumentNumber = "00 000001", CustomerPhoneNumber = "05551234567", CustomerEMail = "john.doe@fake.domain" });
            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerID = 2, CustomerFullName = "Jane Doe", CustomerDocumentNumber = "00 000002", CustomerPhoneNumber = "05550123456", CustomerEMail = "jane.doe@fake.domain" });

            modelBuilder.Entity<Record>().HasData(new Record { RecordID = 1, CustomerID = 1, Status = Status.Closed, Start = new DateTime(2026, 4, 30, 9, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 4, 30, 12, 0, 0, DateTimeKind.Local), Price = 1200 });
            modelBuilder.Entity<Record>().HasData(new Record { RecordID = 2, CustomerID = 2, Status = Status.Scheduled, Start = new DateTime(2026, 5, 31, 16, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 5, 31, 19, 0, 0, DateTimeKind.Local), Price = 750 });
            modelBuilder.Entity<Record>().HasData(new Record { RecordID = 3, CustomerID = 2, Status = Status.Active, Start = new DateTime(2026, 5, 23, 9, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 5, 23, 19, 0, 0, DateTimeKind.Local), Price = 3000 });

            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 1, RecordID = 1, ItemID = 1, PricingID = 1, Status = Status.Closed, Start = new DateTime(2026, 4, 30, 9, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 4, 30, 10, 0, 0, DateTimeKind.Local) });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 2, RecordID = 1, ItemID = 2, PricingID = 1, Status = Status.Closed, Start = new DateTime(2026, 4, 30, 9, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 4, 30, 10, 0, 0, DateTimeKind.Local) });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 3, RecordID = 1, ItemID = 34, PricingID = 26, Status = Status.Closed, Start = new DateTime(2026, 4, 30, 9, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 4, 30, 12, 0, 0, DateTimeKind.Local) });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 4, RecordID = 1, ItemID = 3, PricingID = 1, Status = Status.Closed, Start = new DateTime(2026, 4, 30, 10, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 4, 30, 12, 0, 0, DateTimeKind.Local) });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 5, RecordID = 1, ItemID = 4, PricingID = 1, Status = Status.Closed, Start = new DateTime(2026, 4, 30, 10, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 4, 30, 12, 0, 0, DateTimeKind.Local) });
            
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 6, RecordID = 2, ItemID = 1, PricingID = 1, Status = Status.Scheduled, Start = new DateTime(2026, 5, 31, 16, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 5, 31, 19, 0, 0, DateTimeKind.Local) });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 7, RecordID = 2, ItemID = 34, PricingID = 26, Status = Status.Scheduled, Start = new DateTime(2026, 5, 31, 16, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 5, 31, 19, 0, 0, DateTimeKind.Local) });
            
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 8, RecordID = 3, ItemID = 2, PricingID = 1, Status = Status.Active, Start = new DateTime(2026, 5, 23, 9, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 5, 23, 19, 0, 0, DateTimeKind.Local) });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 9, RecordID = 3, ItemID = 35, PricingID = 26, Status = Status.Active, Start = new DateTime(2026, 5, 23, 9, 0, 0, DateTimeKind.Local), End = new DateTime(2026, 5, 23, 19, 0, 0, DateTimeKind.Local) });
          
            modelBuilder.Entity<Holiday>().HasData(new Holiday { HolidayID = 1, Date = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Local), Name = "Labor Day" });
            modelBuilder.Entity<Holiday>().HasData(new Holiday { HolidayID = 2, Date = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Local), Name = "New Year's Eve" });
            modelBuilder.Entity<Holiday>().HasData(new Holiday { HolidayID = 3, Date = new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Local), Name = "New Year" });
            modelBuilder.Entity<Holiday>().HasData(new Holiday { HolidayID = 4, Date = new DateTime(2027, 5, 1, 0, 0, 0, DateTimeKind.Local), Name = "Labor Day" });

            modelBuilder.Entity<ItemPrepared>().HasData(new ItemPrepared { ItemPreparedID = 1, ItemID = 10 });
            modelBuilder.Entity<ItemPrepared>().HasData(new ItemPrepared { ItemPreparedID = 2, ItemID = 11 });
            modelBuilder.Entity<ItemPrepared>().HasData(new ItemPrepared { ItemPreparedID = 3, ItemID = 31 });
            modelBuilder.Entity<ItemPrepared>().HasData(new ItemPrepared { ItemPreparedID = 4, ItemID = 32 });
        }

        private void UpdateSoftDeleteStatus(EntityEntry entry) { 
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.CurrentValues["isDeleted"] = false;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["isDeleted"] = true;
                    break;
            }        
        }

        public Dictionary<int, List<ItemRecord>> GetAvailability(int? RecordID)
        {
            var availability = ItemRecords.Where(renteditem =>
                    renteditem.RecordID != RecordID &&
                    ((renteditem.Status > Status.Draft && renteditem.Status < Status.Closed) ||
                    (renteditem.Status > Status.Service && renteditem.Status < Status.Fixed)))
                    .ToLookup(
                            renteditem => renteditem.ItemID ?? -1,
                            renteditem => new ItemRecord
                            {
                                Start = renteditem.Start ?? renteditem?.Record?.Start ?? DateTime.MaxValue,
                                End =
                                    (renteditem?.Status == Status.Active || renteditem?.Status == Status.OnService
                                    && (renteditem.End ?? renteditem?.Record?.End ?? DateTime.MinValue) < DateTime.Now) ?
                                    DateTime.Now :
                                    (renteditem?.End ?? renteditem?.Record?.End ?? DateTime.MinValue),
                                Status = renteditem!.Status,
                                ItemRecordID = renteditem.ItemRecordID
                            }
                        ).ToDictionary(x => x.Key, x => x.ToList());

            return availability;
        }


    }
}