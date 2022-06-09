using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bikepark.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw="")
        {
            using (var context = new BikeparkContext( serviceProvider.GetRequiredService<DbContextOptions<BikeparkContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@sevbike.ru");
                await EnsureRole(serviceProvider, adminID, BikeparkConfig.AdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@sevbike.ru");
                await EnsureRole(serviceProvider, managerID, BikeparkConfig.ManagersRole);

                // allowed user can create and edit contacts that they create
                var developerID = await EnsureUser(serviceProvider, testUserPw, "developer@sevbike.ru");
                await EnsureRole(serviceProvider, developerID, BikeparkConfig.ManagersRole);
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
            modelBuilder.Entity<Pricing>()
                .Property(e => e.DaysOfWeek)
                .HasConversion(
                      v => string.Join(",", v.Select(e => e.ToString()).ToArray()),
                      v => v.Split(new[] { ',' })
                        .Select(e => (DayOfWeek)(Enum.Parse(typeof(DayOfWeek), e)) )
                        .Cast<DayOfWeek>()
                        .ToList()
                );
            modelBuilder.Entity<ItemType>().HasQueryFilter(m => EF.Property<bool>(m, "Archival") == false);
            modelBuilder.Entity<Item>().HasQueryFilter(m => EF.Property<bool>(m, "Archival") == false);
            modelBuilder.Entity<Pricing>().HasQueryFilter(m => EF.Property<bool>(m, "Archival") == false);

            List<DayOfWeek> EveryDay = new List<DayOfWeek> {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday,
            };

            List<DayOfWeek> WeekDay = new List<DayOfWeek> {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday,
                    DayOfWeek.Friday,
            };

            List<DayOfWeek> WeekDayShort = new List<DayOfWeek> {
                    DayOfWeek.Monday,
                    DayOfWeek.Tuesday,
                    DayOfWeek.Wednesday,
                    DayOfWeek.Thursday
            };

            List<DayOfWeek> Friday = new List<DayOfWeek> {
                    DayOfWeek.Friday,
            };

            List<DayOfWeek> WeekEnd = new List<DayOfWeek> {
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday,
            };

            List<DayOfWeek> WeekEndLong = new List<DayOfWeek> {
                    DayOfWeek.Friday,
                    DayOfWeek.Saturday,
                    DayOfWeek.Sunday,
            };

            modelBuilder.Entity<ItemCategory>().HasData( new ItemCategory { ItemCategoryID = 1, ItemCategoryName = "MTB" });
            modelBuilder.Entity<ItemCategory>().HasData( new ItemCategory { ItemCategoryID = 2, ItemCategoryName = "MTB подростковый" });
            modelBuilder.Entity<ItemCategory>().HasData( new ItemCategory { ItemCategoryID = 3, ItemCategoryName = "BMX" });
            modelBuilder.Entity<ItemCategory>().HasData( new ItemCategory { ItemCategoryID = 4, ItemCategoryName = "Беговел" });
            modelBuilder.Entity<ItemCategory>().HasData( new ItemCategory { ItemCategoryID = 5, ItemCategoryName = "Электро" });
            modelBuilder.Entity<ItemCategory>().HasData(new ItemCategory { ItemCategoryID = 6, ItemCategoryName = "Аксессуар" });

            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 1, PricingCategoryName = "Горный" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 2, PricingCategoryName = "Подросток" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 3, PricingCategoryName = "BMX" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 4, PricingCategoryName = "Детский" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 5, PricingCategoryName = "Электро" });
            modelBuilder.Entity<PricingCategory>().HasData(new PricingCategory { PricingCategoryID = 6, PricingCategoryName = "Аксессуар" });

            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 1, ItemCategoryID = 1, PricingCategoryID = null, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = "27''", ItemColor = "black", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null }); ;
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 2, ItemCategoryID = null, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = "27''", ItemColor = "aqua", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 3, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = "27''", ItemColor = "black", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 4, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = "27''", ItemColor = "aqua", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 5, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 29 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.XL, ItemWheelSize = "29''", ItemColor = "black", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/2541638.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 6, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "GT AVALANCHE 29 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.XL, ItemWheelSize = "29''", ItemColor = "aqua", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/2541638.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 7, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "Mongoose SWITCHBACK SPORT W", ItemAge = null, ItemGender = ItemGender.W, ItemSize = ItemSize.S, ItemWheelSize = "27.5''", ItemColor = "navy", ItemDescription = "Горный велосипед женский", ItemExternalURL = "https://trial-sport.ru/goods/51516/1496760.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 8, ItemCategoryID = 1, PricingCategoryID = 1, ItemTypeName = "Mongoose SWITCHBACK SPORT W", ItemAge = null, ItemGender = ItemGender.W, ItemSize = ItemSize.M, ItemWheelSize = "27.5''", ItemColor = "navy", ItemDescription = "Горный велосипед женский", ItemExternalURL = "https://trial-sport.ru/goods/51516/1496760.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 9, ItemCategoryID = 2, PricingCategoryID = 2, ItemTypeName = "Mongoose ROCKADILE 20", ItemAge = ItemAge.Teen, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = "20''", ItemColor = "red", ItemDescription = "Горный велосипед подростковый", ItemExternalURL = "https://trial-sport.ru/goods/51516/1493214.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 10, ItemCategoryID = 2, PricingCategoryID = 2, ItemTypeName = "Mongoose ROCKADILE 20 W", ItemAge = ItemAge.Teen, ItemGender = ItemGender.W, ItemSize = ItemSize.M, ItemWheelSize = "20''", ItemColor = "purple", ItemDescription = "Горный велосипед подростковый", ItemExternalURL = "https://trial-sport.ru/goods/51516/1496770.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 11, ItemCategoryID = 3, PricingCategoryID = 3, ItemTypeName = "Radio SAIKO 20", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "black", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2033001.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 12, ItemCategoryID = 3, PricingCategoryID = 3, ItemTypeName = "Radio SAIKO 20", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "metallic purple", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2033001.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 13, ItemCategoryID = 3, PricingCategoryID = 3, ItemTypeName = "Radio DARKO", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "matt black", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2033004.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 14, ItemCategoryID = 3, PricingCategoryID = 3, ItemTypeName = "WeThePeople ARCADE", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "matt black", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2032941.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 15, ItemCategoryID = 4, PricingCategoryID = 4, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "yellow", ItemDescription = "Беговел", ItemExternalURL = "https://trial-sport.ru/goods/51516/1493326.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 16, ItemCategoryID = 4, PricingCategoryID = 4, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "black", ItemDescription = "Беговел", ItemExternalURL = "https://trial-sport.ru/goods/51516/1493326.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 17, ItemCategoryID = 4, PricingCategoryID = 4, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "orange", ItemDescription = "Беговел", ItemExternalURL = "https://trial-sport.ru/goods/51516/1411943.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 18, ItemCategoryID = 4, PricingCategoryID = 4, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "blue", ItemDescription = "Беговел", ItemExternalURL = "https://trial-sport.ru/goods/51516/1411943.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 19, ItemCategoryID = 5, PricingCategoryID = 5, ItemTypeName = "Himo C26", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "26''", ItemColor = null, ItemDescription = "Электровелосипед", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 20, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Шлем", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Шлем", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 21, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Шлем", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Шлем", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 22, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Шлем", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Шлем", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 23, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Шлем", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Шлем детский", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 24, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Наколенники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Наколенники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 25, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Наколенники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Наколенники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 26, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Наколенники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Наколенники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 27, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Наколенники", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Наколенники детские", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 28, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Налокотники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Налокотники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 29, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Налокотники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Налокотники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 30, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Налокотники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Налокотники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 31, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Налокотники", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Налокотники детские", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 32, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Перчатки", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Перчатки", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 33, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Перчатки", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Перчатки", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 34, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Перчатки", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Перчатки", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 35, ItemCategoryID = 6, PricingCategoryID = 6, ItemTypeName = "Перчатки", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Перчатки детские", ItemExternalURL = "", ItemImageURL = null });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 1, ItemTypeID = 1, ItemNumber = "101"});//, ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 2, ItemTypeID = 1, ItemNumber = "102"});//, ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 3, ItemTypeID = 2, ItemNumber = "103"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 4, ItemTypeID = 2, ItemNumber = "104"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 5, ItemTypeID = 3, ItemNumber = "105"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 6, ItemTypeID = 3, ItemNumber = "106"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 7, ItemTypeID = 4, ItemNumber = "107"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 8, ItemTypeID = 4, ItemNumber = "108"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 9, ItemTypeID = 5, ItemNumber = "109"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 10, ItemTypeID = 5, ItemNumber = "110"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 11, ItemTypeID = 5, ItemNumber = "111"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 12, ItemTypeID = 6, ItemNumber = "112"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 13, ItemTypeID = 7, ItemNumber = "113"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 14, ItemTypeID = 8, ItemNumber = "114"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 15, ItemTypeID = 8, ItemNumber = "115"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 16, ItemTypeID = 9, ItemNumber = "116"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 17, ItemTypeID = 9, ItemNumber = "117"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 18, ItemTypeID = 10, ItemNumber = "118"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 19, ItemTypeID = 11, ItemNumber = "201"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 20, ItemTypeID = 12, ItemNumber = "202"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 21, ItemTypeID = 13, ItemNumber = "203"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 22, ItemTypeID = 13, ItemNumber = "204"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 23, ItemTypeID = 14, ItemNumber = "205"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 24, ItemTypeID = 14, ItemNumber = "206"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 25, ItemTypeID = 15, ItemNumber = "301"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 26, ItemTypeID = 16, ItemNumber = "302"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 27, ItemTypeID = 17, ItemNumber = "303"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 28, ItemTypeID = 18, ItemNumber = "304"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 29, ItemTypeID = 19, ItemNumber = "401"});//, ItemStatus = ItemStatus.RentedOut });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 30, ItemTypeID = 19, ItemNumber = "402"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 31, ItemTypeID = 19, ItemNumber = "403"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 32, ItemTypeID = 19, ItemNumber = "404"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 33, ItemTypeID = 19, ItemNumber = "405"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 34, ItemTypeID = 20, ItemNumber = "1"});//, ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 35, ItemTypeID = 21, ItemNumber = "2"});//, ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 36, ItemTypeID = 21, ItemNumber = "3"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 37, ItemTypeID = 22, ItemNumber = "4"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 38, ItemTypeID = 23, ItemNumber = "5"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 39, ItemTypeID = 23, ItemNumber = "6"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 40, ItemTypeID = 24, ItemNumber = "7"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 41, ItemTypeID = 25, ItemNumber = "8"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 42, ItemTypeID = 25, ItemNumber = "9"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 43, ItemTypeID = 26, ItemNumber = "10"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 44, ItemTypeID = 27, ItemNumber = "11"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 45, ItemTypeID = 27, ItemNumber = "12"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 46, ItemTypeID = 28, ItemNumber = "13"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 47, ItemTypeID = 29, ItemNumber = "14"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 48, ItemTypeID = 29, ItemNumber = "15"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 49, ItemTypeID = 30, ItemNumber = "16"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 50, ItemTypeID = 31, ItemNumber = "17"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 51, ItemTypeID = 31, ItemNumber = "18"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 52, ItemTypeID = 32, ItemNumber = "19"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 53, ItemTypeID = 33, ItemNumber = "20"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 54, ItemTypeID = 33, ItemNumber = "21"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 55, ItemTypeID = 34, ItemNumber = "22"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 56, ItemTypeID = 35, ItemNumber = "23"});//, ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 57, ItemTypeID = 35, ItemNumber = "24"});//, ItemStatus = ItemStatus.Available });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 1, PricingName = "MTB будний час", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 2, PricingName = "MTB пятница час", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = Friday, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 3, PricingName = "MTB выходной час", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = WeekEnd, IsHoliday = false, IsReduced = false, Price = 200 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 4, PricingName = "MTB праздник час", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 200 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 5, PricingName = "MTB льготный час", PricingCategoryID = null, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = true, Price = 100 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 28, PricingName = "MTB будний 2 часа", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 250/2, MinDuration = 2});
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 29, PricingName = "MTB пятница 2 часа", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = Friday, IsHoliday = false, IsReduced = false, Price = 250/2, MinDuration = 2 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 30, PricingName = "MTB выходной 2 часа", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = WeekEnd, IsHoliday = false, IsReduced = false, Price = 300/2, MinDuration = 2 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 31, PricingName = "MTB праздник 2 часа", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 300/2, MinDuration = 2 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 32, PricingName = "MTB льготный 2 часа", PricingCategoryID = 1, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = true, Price = 150/2, MinDuration = 2 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 6, PricingName = "MTB будний день", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 800 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 7, PricingName = "MTB пятница день", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = Friday, IsHoliday = false, IsReduced = false, Price = 1000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 8, PricingName = "MTB выходной день", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = WeekEnd, IsHoliday = false, IsReduced = false, Price = 1000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 9, PricingName = "MTB праздник день", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 1000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 10, PricingName = "MTB льготный день", PricingCategoryID = 1, PricingType = PricingType.OneTime, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = true, Price = 400 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 11, PricingName = "MTB подросток будний час", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 12, PricingName = "MTB подросток пятница час", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = Friday, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 13, PricingName = "MTB подросток выходной час", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = WeekEnd, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 14, PricingName = "MTB подросток праздник час", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 15, PricingName = "MTB подросток льготный час", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = true, Price = 50 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 16, PricingName = "MTB подросток будний день", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 500 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 17, PricingName = "MTB подросток пятница день", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = Friday, IsHoliday = false, IsReduced = false, Price = 600 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 18, PricingName = "MTB подросток выходной день", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = WeekEnd, IsHoliday = false, IsReduced = false, Price = 700 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 19, PricingName = "MTB подросток праздник день", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 700 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 20, PricingName = "MTB подросток льготный день", PricingCategoryID = 2, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = true, Price = 300 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 21, PricingName = "BMX будний час", PricingCategoryID = 3, PricingType = PricingType.Hourly, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 22, PricingName = "BMX пт-сб-вс час", PricingCategoryID = 3, PricingType = PricingType.Hourly, DaysOfWeek = WeekEndLong, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 23, PricingName = "BMX праздник час", PricingCategoryID = 3, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 150 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 24, PricingName = "Беговел час", PricingCategoryID = 4, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 25, PricingName = "Электро час", PricingCategoryID = 5, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = false, Price = 300 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 26, PricingName = "Аксессуар час", PricingCategoryID = 6, PricingType = PricingType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = false, Price = 50 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 27, PricingName = "Аксессуар день", PricingCategoryID = 6, PricingType = PricingType.OneTime, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = false, Price = 300 });

            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 33, PricingName = "Ремонт колеса", PricingCategoryID = null, PricingType = PricingType.Service, Price = 2000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 34, PricingName = "Ремонт цепи", PricingCategoryID = null, PricingType = PricingType.Service, Price = 2000 });
            modelBuilder.Entity<Pricing>().HasData(new Pricing { PricingID = 35, PricingName = "Ремонт тормоза", PricingCategoryID = null, PricingType = PricingType.Service, Price = 1000 });

            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerID = 1, CustomerFullName = "Петр Иванов", CustomerPassport = "00 000001", CustomerContactNumber = "+79781234567", CustomerEMail = "vasily.pupkin@maily.su" });
            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerID = 2, CustomerFullName = "Иван Петров", CustomerPassport = "00 000002", CustomerContactNumber = "+79780123456", CustomerEMail = "ivan.petrov@maily.su" });

            modelBuilder.Entity<Record>().HasData(new Record { RecordID = 1, CustomerID = 1, Status = Status.Closed   , Start = DateTime.Parse("19.04.2022 09:00"), End = DateTime.Parse("19.04.2022 12:00"), Price = 1200 });
            modelBuilder.Entity<Record>().HasData(new Record { RecordID = 2, CustomerID = 2, Status = Status.Scheduled, Start = DateTime.Parse("15.05.2022 16:00"), End = DateTime.Parse("15.05.2022 19:00"), Price = 750 });
            modelBuilder.Entity<Record>().HasData(new Record { RecordID = 3, CustomerID = 1, Status = Status.Scheduled, Start = DateTime.Parse("15.05.2022 16:00"), End = DateTime.Parse("15.05.2022 19:00"), Price = 750 });
            modelBuilder.Entity<Record>().HasData(new Record { RecordID = 4, CustomerID = 1, Status = Status.Scheduled, Start = DateTime.Parse("16.05.2022 10:00"), End = DateTime.Parse("16.05.2022 14:00"), Price = 1200 });
            modelBuilder.Entity<Record>().HasData(new Record { RecordID = 5, CustomerID = 2, Status = Status.Active   , Start = DateTime.Parse("14.05.2022 18:00"), End = DateTime.Parse("14.05.2022 23:00"), Price = 3000 });

            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 1, RecordID = 1, ItemID = 1,    PricingID = 1,  Status = Status.Closed, Start = DateTime.Parse("19.04.2022 09:00"), End = DateTime.Parse("19.04.2022 12:00") });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 2, RecordID = 1, ItemID = 2,    PricingID = 1,  Status = Status.Closed, Start = DateTime.Parse("19.04.2022 09:00"), End = DateTime.Parse("19.04.2022 11:00") });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 3, RecordID = 1, ItemID = 3,    PricingID = 1,  Status = Status.Closed, Start = DateTime.Parse("19.04.2022 10:00"), End = DateTime.Parse("19.04.2022 12:00") });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 4, RecordID = 1, ItemID = 4,    PricingID = 1,  Status = Status.Closed, Start = DateTime.Parse("19.04.2022 10:00"), End = DateTime.Parse("19.04.2022 11:00") });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 5, RecordID = 1, ItemID = 34,   PricingID = 26, Status = Status.Closed, Start = DateTime.Parse("19.04.2022 09:00"), End = DateTime.Parse("19.04.2022 12:00") });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 6, RecordID = 2, ItemID = 1,    PricingID = 1,  Status = Status.Scheduled, Start = DateTime.Parse("15.05.2022 16:00"), End = DateTime.Parse("15.05.2022 19:00") });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 7, RecordID = 2, ItemID = 34,   PricingID = 26, Status = Status.Scheduled, Start = DateTime.Parse("15.05.2022 16:00"), End = DateTime.Parse("15.05.2022 19:00")  });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 8, RecordID = 3, ItemID = 2,    PricingID = 1,  Status = Status.Scheduled, Start = DateTime.Parse("15.05.2022 16:00"), End = DateTime.Parse("15.05.2022 19:00")  });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 9, RecordID = 3, ItemID = 35,   PricingID = 26, Status = Status.Scheduled, Start = DateTime.Parse("15.05.2022 16:00"), End = DateTime.Parse("15.05.2022 19:00")  });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 10, RecordID = 4, ItemID = 29,  PricingID = 25, Status = Status.Scheduled, Start = DateTime.Parse("16.05.2022 10:00"), End = DateTime.Parse("16.05.2022 14:00") });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 11, RecordID = 5, ItemID = 29,  PricingID = 25, Status = Status.Active, Start = DateTime.Parse("14.05.2022 18:00"), End = DateTime.Parse("14.05.2022 23:00") });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 12, RecordID = 5, ItemID = 30,  PricingID = 25, Status = Status.Closed, Start = DateTime.Parse("14.05.2022 18:00"), End = DateTime.Parse("14.05.2022 19:00") });
            modelBuilder.Entity<ItemRecord>().HasData(new ItemRecord { ItemRecordID = 13, RecordID = 5, ItemID = 31,  PricingID = 25, Status = Status.Active, Start = DateTime.Parse("14.05.2022 19:00"), End = DateTime.Parse("14.05.2022 23:00") });

            modelBuilder.Entity<Holiday>().HasData(new Holiday { HolidayID = 1, Date = DateTime.Parse("12.06.2022"), Name = "День России" });
            modelBuilder.Entity<ItemPrepared>().HasData(new ItemPrepared { ItemPreparedID = 1, ItemID = 10 });
            modelBuilder.Entity<ItemPrepared>().HasData(new ItemPrepared { ItemPreparedID = 2, ItemID = 11 });
            modelBuilder.Entity<ItemPrepared>().HasData(new ItemPrepared { ItemPreparedID = 3, ItemID = 31 });
            modelBuilder.Entity<ItemPrepared>().HasData(new ItemPrepared { ItemPreparedID = 4, ItemID = 32 });

        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            //LinqHelper.ForEach(ChangeTracker.Entries<ItemCategory>().Where(e => e.State == EntityState.Deleted), e => CascadeClearItemCategory(e));
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

    }
}