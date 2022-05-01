using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bikepark.Models;
using Bikepark.Authorization;

namespace Bikepark.Data
{
    public class BikeparkContext : IdentityDbContext
    {
        public DbSet<Item> Storage { get; set; }
        public DbSet<RentalRecord> RentalLog { get; set; }
        public DbSet<ServiceRecord> ServiceLog { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public BikeparkContext(DbContextOptions<BikeparkContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw="")
        {
            using (var context = new BikeparkContext( serviceProvider.GetRequiredService<DbContextOptions<BikeparkContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@bikepark.ru");
                await EnsureRole(serviceProvider, adminID, Constants.AdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@bikepark.ru");
                await EnsureRole(serviceProvider, managerID, Constants.ManagersRole);
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
            //var culture = new System.Globalization.CultureInfo("ru-RU");
            //var DayOfWeek = culture.DateTimeFormat.GetAbbreviatedDayName(DateTime.Today.DayOfWeek);
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasIndex(p => p.ItemID) // не Null
                .IsUnique();
            modelBuilder.Entity<RentalPricing>()
                .Property(e => e.DaysOfWeek)
                .HasConversion(
                      v => string.Join(",", v.Select(e => e.DayOfWeek.ToString()).ToArray()),
                      v => v.Split(new[] { ',' })
                        .Select(e => DayOfWeekRu.Map.GetValueOrDefault((DayOfWeek)(Enum.Parse(typeof(DayOfWeek), e))) )
                        .Cast<DayOfWeekRu>()
                        .ToList()
                );



            ItemCategory MTB, MTBTeen, BMX, Balance, Electro, Accessories;
   
            List<DayOfWeekRu> EveryDay = new List<DayOfWeekRu> {
                    DayOfWeekRu.Monday,
                    DayOfWeekRu.Tuesday,
                    DayOfWeekRu.Wednesday,
                    DayOfWeekRu.Thursday,
                    DayOfWeekRu.Friday,
                    DayOfWeekRu.Saturday,
                    DayOfWeekRu.Sunday,
            };

            List<DayOfWeekRu> WeekDay = new List<DayOfWeekRu> {
                    DayOfWeekRu.Monday,
                    DayOfWeekRu.Tuesday,
                    DayOfWeekRu.Wednesday,
                    DayOfWeekRu.Thursday,
                    DayOfWeekRu.Friday,
            };

            List<DayOfWeekRu> WeekDayShort = new List<DayOfWeekRu> {
                    DayOfWeekRu.Monday,
                    DayOfWeekRu.Tuesday,
                    DayOfWeekRu.Wednesday,
                    DayOfWeekRu.Thursday
            };

            List<DayOfWeekRu> Friday = new List<DayOfWeekRu> {
                    DayOfWeekRu.Friday,
            };

            List<DayOfWeekRu> WeekEnd = new List<DayOfWeekRu> {
                    DayOfWeekRu.Saturday,
                    DayOfWeekRu.Sunday,
            };

            List<DayOfWeekRu> WeekEndLong = new List<DayOfWeekRu> {
                    DayOfWeekRu.Friday,
                    DayOfWeekRu.Saturday,
                    DayOfWeekRu.Sunday,
            };

            modelBuilder.Entity<ItemCategory>().HasData(MTB = new ItemCategory { ItemCategoryID = 1, ItemCategoryName = "MTB" });
            modelBuilder.Entity<ItemCategory>().HasData(MTBTeen = new ItemCategory { ItemCategoryID = 2, ItemCategoryName = "MTB подростковый" });
            modelBuilder.Entity<ItemCategory>().HasData(BMX = new ItemCategory { ItemCategoryID = 3, ItemCategoryName = "BMX" });
            modelBuilder.Entity<ItemCategory>().HasData(Balance = new ItemCategory { ItemCategoryID = 4, ItemCategoryName = "Беговел" });
            modelBuilder.Entity<ItemCategory>().HasData(Electro = new ItemCategory { ItemCategoryID = 5, ItemCategoryName = "Электровелосипед" });
            modelBuilder.Entity<ItemCategory>().HasData(Accessories = new ItemCategory { ItemCategoryID = 6, ItemCategoryName = "Аксессуар" });

            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 1, ItemCategoryID = MTB.ItemCategoryID, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = "27''", ItemColor = "black", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null }); ;
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 2, ItemCategoryID = MTB.ItemCategoryID, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = "27''", ItemColor = "aqua", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 3, ItemCategoryID = MTB.ItemCategoryID, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = "27''", ItemColor = "black", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 4, ItemCategoryID = MTB.ItemCategoryID, ItemTypeName = "GT AVALANCHE 27 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = "27''", ItemColor = "aqua", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/1490897.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 5, ItemCategoryID = MTB.ItemCategoryID, ItemTypeName = "GT AVALANCHE 29 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.XL, ItemWheelSize = "29''", ItemColor = "black", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/2541638.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 6, ItemCategoryID = MTB.ItemCategoryID, ItemTypeName = "GT AVALANCHE 29 SPORT", ItemAge = null, ItemGender = null, ItemSize = ItemSize.XL, ItemWheelSize = "29''", ItemColor = "aqua", ItemDescription = "Горный велосипед", ItemExternalURL = "https://trial-sport.ru/goods/51516/2541638.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 7, ItemCategoryID = MTB.ItemCategoryID, ItemTypeName = "Mongoose SWITCHBACK SPORT W", ItemAge = null, ItemGender = ItemGender.W, ItemSize = ItemSize.S, ItemWheelSize = "27.5''", ItemColor = "navy", ItemDescription = "Горный велосипед женский", ItemExternalURL = "https://trial-sport.ru/goods/51516/1496760.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 8, ItemCategoryID = MTB.ItemCategoryID, ItemTypeName = "Mongoose SWITCHBACK SPORT W", ItemAge = null, ItemGender = ItemGender.W, ItemSize = ItemSize.M, ItemWheelSize = "27.5''", ItemColor = "navy", ItemDescription = "Горный велосипед женский", ItemExternalURL = "https://trial-sport.ru/goods/51516/1496760.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 9, ItemCategoryID = MTBTeen.ItemCategoryID, ItemTypeName = "Mongoose ROCKADILE 20", ItemAge = ItemAge.Teen, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = "20''", ItemColor = "red", ItemDescription = "Горный велосипед подростковый", ItemExternalURL = "https://trial-sport.ru/goods/51516/1493214.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 10, ItemCategoryID = MTBTeen.ItemCategoryID, ItemTypeName = "Mongoose ROCKADILE 20 W", ItemAge = ItemAge.Teen, ItemGender = ItemGender.W, ItemSize = ItemSize.M, ItemWheelSize = "20''", ItemColor = "purple", ItemDescription = "Горный велосипед подростковый", ItemExternalURL = "https://trial-sport.ru/goods/51516/1496770.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 11, ItemCategoryID = BMX.ItemCategoryID, ItemTypeName = "Radio SAIKO 20", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "black", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2033001.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 12, ItemCategoryID = BMX.ItemCategoryID, ItemTypeName = "Radio SAIKO 20", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "metallic purple", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2033001.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 13, ItemCategoryID = BMX.ItemCategoryID, ItemTypeName = "Radio DARKO", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "matt black", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2033004.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 14, ItemCategoryID = BMX.ItemCategoryID, ItemTypeName = "WeThePeople ARCADE", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "20''", ItemColor = "matt black", ItemDescription = "BMX", ItemExternalURL = "https://trial-sport.ru/goods/51516/2032941.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 15, ItemCategoryID = Balance.ItemCategoryID, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "yellow", ItemDescription = "Беговел", ItemExternalURL = "https://trial-sport.ru/goods/51516/1493326.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 16, ItemCategoryID = Balance.ItemCategoryID, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "black", ItemDescription = "Беговел", ItemExternalURL = "https://trial-sport.ru/goods/51516/1493326.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 17, ItemCategoryID = Balance.ItemCategoryID, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "orange", ItemDescription = "Беговел", ItemExternalURL = "https://trial-sport.ru/goods/51516/1411943.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 18, ItemCategoryID = Balance.ItemCategoryID, ItemTypeName = "Outleap ROCKET", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = "12''", ItemColor = "blue", ItemDescription = "Беговел", ItemExternalURL = "https://trial-sport.ru/goods/51516/1411943.html", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 19, ItemCategoryID = Electro.ItemCategoryID, ItemTypeName = "Himo C26", ItemAge = null, ItemGender = null, ItemSize = null, ItemWheelSize = "26''", ItemColor = null, ItemDescription = "Электровелосипед", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 20, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Шлем", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Шлем", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 21, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Шлем", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Шлем", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 22, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Шлем", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Шлем", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 23, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Шлем", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Шлем детский", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 24, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Наколенники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Наколенники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 25, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Наколенники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Наколенники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 26, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Наколенники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Наколенники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 27, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Наколенники", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Наколенники детские", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 28, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Налокотники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Налокотники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 29, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Налокотники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Налокотники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 30, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Налокотники", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Налокотники", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 31, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Налокотники", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Налокотники детские", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 32, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Перчатки", ItemAge = null, ItemGender = null, ItemSize = ItemSize.L, ItemWheelSize = null, ItemColor = null, ItemDescription = "Перчатки", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 33, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Перчатки", ItemAge = null, ItemGender = null, ItemSize = ItemSize.M, ItemWheelSize = null, ItemColor = null, ItemDescription = "Перчатки", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 34, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Перчатки", ItemAge = null, ItemGender = null, ItemSize = ItemSize.S, ItemWheelSize = null, ItemColor = null, ItemDescription = "Перчатки", ItemExternalURL = "", ItemImageURL = null });
            modelBuilder.Entity<ItemType>().HasData(new ItemType { ItemTypeID = 35, ItemCategoryID = Accessories.ItemCategoryID, ItemTypeName = "Перчатки", ItemAge = ItemAge.Kid, ItemGender = null, ItemSize = null, ItemWheelSize = null, ItemColor = null, ItemDescription = "Перчатки детские", ItemExternalURL = "", ItemImageURL = null });

            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 1, ItemTypeID = 1, ItemNumber = "101", ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 2, ItemTypeID = 1, ItemNumber = "102", ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 3, ItemTypeID = 2, ItemNumber = "103", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 4, ItemTypeID = 2, ItemNumber = "104", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 5, ItemTypeID = 3, ItemNumber = "105", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 6, ItemTypeID = 3, ItemNumber = "106", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 7, ItemTypeID = 4, ItemNumber = "107", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 8, ItemTypeID = 4, ItemNumber = "108", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 9, ItemTypeID = 5, ItemNumber = "109", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 10, ItemTypeID = 5, ItemNumber = "110", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 11, ItemTypeID = 5, ItemNumber = "111", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 12, ItemTypeID = 6, ItemNumber = "112", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 13, ItemTypeID = 7, ItemNumber = "113", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 14, ItemTypeID = 8, ItemNumber = "114", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 15, ItemTypeID = 8, ItemNumber = "115", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 16, ItemTypeID = 9, ItemNumber = "116", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 17, ItemTypeID = 9, ItemNumber = "117", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 18, ItemTypeID = 10, ItemNumber = "118", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 19, ItemTypeID = 11, ItemNumber = "201", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 20, ItemTypeID = 12, ItemNumber = "202", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 21, ItemTypeID = 13, ItemNumber = "203", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 22, ItemTypeID = 13, ItemNumber = "204", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 23, ItemTypeID = 14, ItemNumber = "205", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 24, ItemTypeID = 14, ItemNumber = "206", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 25, ItemTypeID = 15, ItemNumber = "301", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 26, ItemTypeID = 16, ItemNumber = "302", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 27, ItemTypeID = 17, ItemNumber = "303", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 28, ItemTypeID = 18, ItemNumber = "304", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 29, ItemTypeID = 19, ItemNumber = "401", ItemStatus = ItemStatus.RentedOut });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 30, ItemTypeID = 19, ItemNumber = "402", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 31, ItemTypeID = 19, ItemNumber = "403", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 32, ItemTypeID = 19, ItemNumber = "404", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 33, ItemTypeID = 19, ItemNumber = "405", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 34, ItemTypeID = 20, ItemNumber = "1", ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 35, ItemTypeID = 21, ItemNumber = "2", ItemStatus = ItemStatus.Reserved });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 36, ItemTypeID = 21, ItemNumber = "3", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 37, ItemTypeID = 22, ItemNumber = "4", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 38, ItemTypeID = 23, ItemNumber = "5", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 39, ItemTypeID = 23, ItemNumber = "6", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 40, ItemTypeID = 24, ItemNumber = "7", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 41, ItemTypeID = 25, ItemNumber = "8", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 42, ItemTypeID = 25, ItemNumber = "9", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 43, ItemTypeID = 26, ItemNumber = "10", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 44, ItemTypeID = 27, ItemNumber = "11", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 45, ItemTypeID = 27, ItemNumber = "12", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 46, ItemTypeID = 28, ItemNumber = "13", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 47, ItemTypeID = 29, ItemNumber = "14", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 48, ItemTypeID = 29, ItemNumber = "15", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 49, ItemTypeID = 30, ItemNumber = "16", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 50, ItemTypeID = 31, ItemNumber = "17", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 51, ItemTypeID = 31, ItemNumber = "18", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 52, ItemTypeID = 32, ItemNumber = "19", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 53, ItemTypeID = 33, ItemNumber = "20", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 54, ItemTypeID = 33, ItemNumber = "21", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 55, ItemTypeID = 34, ItemNumber = "22", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 56, ItemTypeID = 35, ItemNumber = "23", ItemStatus = ItemStatus.Available });
            modelBuilder.Entity<Item>().HasData(new Item { ItemID = 57, ItemTypeID = 35, ItemNumber = "24", ItemStatus = ItemStatus.Available });

            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 1, RentalPricingName = "MTB будний час", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 2, RentalPricingName = "MTB пятница час", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = Friday, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 3, RentalPricingName = "MTB выходной час", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = WeekEnd, IsHoliday = false, IsReduced = false, Price = 200 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 4, RentalPricingName = "MTB праздник час", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 200 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 5, RentalPricingName = "MTB льготный час", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = true, Price = 100 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 6, RentalPricingName = "MTB будний день", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Daily, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 800 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 7, RentalPricingName = "MTB пятница день", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Daily, DaysOfWeek = Friday, IsHoliday = false, IsReduced = false, Price = 1000 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 8, RentalPricingName = "MTB выходной день", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Daily, DaysOfWeek = WeekEnd, IsHoliday = false, IsReduced = false, Price = 1000 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 9, RentalPricingName = "MTB праздник день", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Daily, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 1000 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 10, RentalPricingName = "MTB льготный день", ItemCategoryID = MTB.ItemCategoryID, RentalType = RentalType.Daily, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = true, Price = 400 });

            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 11, RentalPricingName = "MTB подросток будний час", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 12, RentalPricingName = "MTB подросток пятница час", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = Friday, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 13, RentalPricingName = "MTB подросток выходной час", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = WeekEnd, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 14, RentalPricingName = "MTB подросток праздник час", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 150 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 15, RentalPricingName = "MTB подросток льготный час", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = true, Price = 50 });

            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 16, RentalPricingName = "MTB подросток будний день", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 500 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 17, RentalPricingName = "MTB подросток пятница день", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = Friday, IsHoliday = false, IsReduced = false, Price = 600 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 18, RentalPricingName = "MTB подросток выходной день", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = WeekEnd, IsHoliday = false, IsReduced = false, Price = 700 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 19, RentalPricingName = "MTB подросток праздник день", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 700 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 20, RentalPricingName = "MTB подросток льготный день", ItemCategoryID = MTBTeen.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = true, Price = 300 });

            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 21, RentalPricingName = "BMX будний час", ItemCategoryID = BMX.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = WeekDayShort, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 22, RentalPricingName = "BMX пт-сб-вс час", ItemCategoryID = BMX.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = WeekEndLong, IsHoliday = false, IsReduced = false, Price = 150 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 23, RentalPricingName = "BMX праздник час", ItemCategoryID = BMX.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = true, IsReduced = false, Price = 150 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 24, RentalPricingName = "Беговел час", ItemCategoryID = Balance.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = false, Price = 100 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 25, RentalPricingName = "Электро час", ItemCategoryID = Electro.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = false, Price = 300 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 26, RentalPricingName = "Аксессуар час", ItemCategoryID = Accessories.ItemCategoryID, RentalType = RentalType.Hourly, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = false, Price = 50 });
            modelBuilder.Entity<RentalPricing>().HasData(new RentalPricing { RentalPricingID = 27, RentalPricingName = "Аксессуар день", ItemCategoryID = Accessories.ItemCategoryID, RentalType = RentalType.Daily, DaysOfWeek = EveryDay, IsHoliday = false, IsReduced = false, Price = 300 });

            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerID = 1, CustomerFullName = "Василий Пупкин", CustomerPassport = "00 000000", CustomerContactNumber = "+79781234567", CustomerEMail = "vasily.pupkin@maily.su" });
            modelBuilder.Entity<Customer>().HasData(new Customer { CustomerID = 2, CustomerFullName = "Иван Петров", CustomerPassport = "11 000000", CustomerContactNumber = "+79780123456", CustomerEMail = "ivan.petrov@maily.su" });

            modelBuilder.Entity<RentalRecord>().HasData(new RentalRecord { RentalRecordID = 1, CustomerID = 1, RentalStatus = RentalStatus.Closed, RentalType = RentalType.Hourly,      Start = DateTime.Parse("19.04.2022 09:00"), End = DateTime.Parse("19.04.2022 12:00") });
            modelBuilder.Entity<RentalRecord>().HasData(new RentalRecord { RentalRecordID = 2, CustomerID = 2, RentalStatus = RentalStatus.Scheduled, RentalType = RentalType.Hourly,   Start = DateTime.Parse("30.04.2022 16:00"), End = DateTime.Parse("30.04.2022 19:00") });
            modelBuilder.Entity<RentalRecord>().HasData(new RentalRecord { RentalRecordID = 3, CustomerID = 1, RentalStatus = RentalStatus.Scheduled, RentalType = RentalType.Hourly,   Start = DateTime.Parse("30.04.2022 16:00"), End = DateTime.Parse("30.04.2022 19:00") });
            modelBuilder.Entity<RentalRecord>().HasData(new RentalRecord { RentalRecordID = 4, CustomerID = 1, RentalStatus = RentalStatus.Scheduled, RentalType = RentalType.Hourly,   Start = DateTime.Parse("01.05.2022 10:00"), End = DateTime.Parse("1.05.2022 14:00") });
            modelBuilder.Entity<RentalRecord>().HasData(new RentalRecord { RentalRecordID = 5, CustomerID = 2, RentalStatus = RentalStatus.Active, RentalType = RentalType.Daily,       Start = DateTime.Parse("30.04.2022 08:00"), End = DateTime.Parse("1.05.2022 08:00") });
            modelBuilder.Entity<RentalRecord>().HasData(new RentalRecord { RentalRecordID = 6, CustomerID = null, RentalStatus = null, RentalType = RentalType.OneTime, Start = null, End = null });

            //modelBuilder.Entity<PaymentOrRefundRecord>().HasData(new PaymentOrRefundRecord { PaymentOrRefundRecordID = 1, DateTime = DateTime.Parse("19.04.2022 08:00"), Value = 450, Executed = true, RentalRecordID = 1, ServiceRecordID = null});
            //modelBuilder.Entity<PaymentOrRefundRecord>().HasData(new PaymentOrRefundRecord { PaymentOrRefundRecordID = 2, DateTime = DateTime.Parse("19.04.2022 15:00"), Value = 750, Executed = true, RentalRecordID = 2, ServiceRecordID = null });
            //modelBuilder.Entity<PaymentOrRefundRecord>().HasData(new PaymentOrRefundRecord { PaymentOrRefundRecordID = 3, DateTime = DateTime.Parse("19.04.2022 15:00"), Value = 500, Executed = true, RentalRecordID = 3, ServiceRecordID = null });

            modelBuilder.Entity<RentalItem>().HasData(new RentalItem { RentalItemID = 1, RentalRecordID = 1, ItemID = 1,    RentalPricingID = 1,  RentalStatus = RentalStatus.Closed });
            modelBuilder.Entity<RentalItem>().HasData(new RentalItem { RentalItemID = 2, RentalRecordID = 1, ItemID = 34,   RentalPricingID = 18, RentalStatus = RentalStatus.Closed });
            modelBuilder.Entity<RentalItem>().HasData(new RentalItem { RentalItemID = 3, RentalRecordID = 2, ItemID = 1,    RentalPricingID = 1,  RentalStatus = RentalStatus.Scheduled });
            modelBuilder.Entity<RentalItem>().HasData(new RentalItem { RentalItemID = 4, RentalRecordID = 2, ItemID = 34,   RentalPricingID = 18, RentalStatus = RentalStatus.Scheduled });
            modelBuilder.Entity<RentalItem>().HasData(new RentalItem { RentalItemID = 5, RentalRecordID = 3, ItemID = 2,    RentalPricingID = 1,  RentalStatus = RentalStatus.Scheduled });
            modelBuilder.Entity<RentalItem>().HasData(new RentalItem { RentalItemID = 6, RentalRecordID = 3, ItemID = 35,   RentalPricingID = 18, RentalStatus = RentalStatus.Scheduled });
            modelBuilder.Entity<RentalItem>().HasData(new RentalItem { RentalItemID = 7, RentalRecordID = 4, ItemID = 29,   RentalPricingID = 17, RentalStatus = RentalStatus.Scheduled });
            modelBuilder.Entity<RentalItem>().HasData(new RentalItem { RentalItemID = 8, RentalRecordID = 5, ItemID = 29,   RentalPricingID = 17, RentalStatus = RentalStatus.Active });



        }

        public DbSet<Bikepark.Models.Customer> Customer { get; set; }

    }
}