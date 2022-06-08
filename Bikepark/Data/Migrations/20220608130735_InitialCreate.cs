using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bikepark.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerContactNumber = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerFullName = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerPassport = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerEMail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    HolidayID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.HolidayID);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    ItemCategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemCategoryName = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => x.ItemCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "PricingCategories",
                columns: table => new
                {
                    PricingCategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PricingCategoryName = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingCategories", x => x.PricingCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    RecordID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerID = table.Column<int>(type: "INTEGER", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: true),
                    End = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CustomInformation = table.Column<string>(type: "TEXT", nullable: true),
                    UserID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.RecordID);
                    table.ForeignKey(
                        name: "FK_Records_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Records_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    ItemTypeID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemTypeName = table.Column<string>(type: "TEXT", nullable: false),
                    ItemCategoryID = table.Column<int>(type: "INTEGER", nullable: true),
                    PricingCategoryID = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemAge = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemGender = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemSize = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemWheelSize = table.Column<string>(type: "TEXT", nullable: true),
                    ItemColor = table.Column<string>(type: "TEXT", nullable: true),
                    ItemDescription = table.Column<string>(type: "TEXT", nullable: true),
                    ItemExternalURL = table.Column<string>(type: "TEXT", nullable: true),
                    ItemImageURL = table.Column<string>(type: "TEXT", nullable: true),
                    Archival = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.ItemTypeID);
                    table.ForeignKey(
                        name: "FK_ItemTypes_ItemCategories_ItemCategoryID",
                        column: x => x.ItemCategoryID,
                        principalTable: "ItemCategories",
                        principalColumn: "ItemCategoryID");
                    table.ForeignKey(
                        name: "FK_ItemTypes_PricingCategories_PricingCategoryID",
                        column: x => x.PricingCategoryID,
                        principalTable: "PricingCategories",
                        principalColumn: "PricingCategoryID");
                });

            migrationBuilder.CreateTable(
                name: "Pricings",
                columns: table => new
                {
                    PricingID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PricingName = table.Column<string>(type: "TEXT", nullable: true),
                    PricingCategoryID = table.Column<int>(type: "INTEGER", nullable: true),
                    PricingType = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysOfWeek = table.Column<string>(type: "TEXT", nullable: false),
                    IsHoliday = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsReduced = table.Column<bool>(type: "INTEGER", nullable: false),
                    MinDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Archival = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricings", x => x.PricingID);
                    table.ForeignKey(
                        name: "FK_Pricings_PricingCategories_PricingCategoryID",
                        column: x => x.PricingCategoryID,
                        principalTable: "PricingCategories",
                        principalColumn: "PricingCategoryID");
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemTypeID = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Archival = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_ItemTypeID",
                        column: x => x.ItemTypeID,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeID");
                });

            migrationBuilder.CreateTable(
                name: "ItemRecords",
                columns: table => new
                {
                    ItemRecordID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecordID = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemID = table.Column<int>(type: "INTEGER", nullable: true),
                    PricingID = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: true),
                    End = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CustomInformation = table.Column<string>(type: "TEXT", nullable: true),
                    UserID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemRecords", x => x.ItemRecordID);
                    table.ForeignKey(
                        name: "FK_ItemRecords_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemRecords_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID");
                    table.ForeignKey(
                        name: "FK_ItemRecords_Pricings_PricingID",
                        column: x => x.PricingID,
                        principalTable: "Pricings",
                        principalColumn: "PricingID");
                    table.ForeignKey(
                        name: "FK_ItemRecords_Records_RecordID",
                        column: x => x.RecordID,
                        principalTable: "Records",
                        principalColumn: "RecordID");
                });

            migrationBuilder.CreateTable(
                name: "Prepared",
                columns: table => new
                {
                    ItemPreparedID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prepared", x => x.ItemPreparedID);
                    table.ForeignKey(
                        name: "FK_Prepared_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "CustomerContactNumber", "CustomerEMail", "CustomerFullName", "CustomerPassport" },
                values: new object[] { 1, "+79781234567", "vasily.pupkin@maily.su", "Василий Пупкин", "00 000001" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "CustomerContactNumber", "CustomerEMail", "CustomerFullName", "CustomerPassport" },
                values: new object[] { 2, "+79780123456", "ivan.petrov@maily.su", "Иван Петров", "00 000002" });

            migrationBuilder.InsertData(
                table: "Holidays",
                columns: new[] { "HolidayID", "Date", "Name" },
                values: new object[] { 1, new DateTime(2022, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "День России" });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 1, "MTB" });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 2, "MTB подростковый" });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 3, "BMX" });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 4, "Беговел" });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 5, "Электро" });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 6, "Аксессуар" });

            migrationBuilder.InsertData(
                table: "PricingCategories",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 1, "Горный" });

            migrationBuilder.InsertData(
                table: "PricingCategories",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 2, "Подросток" });

            migrationBuilder.InsertData(
                table: "PricingCategories",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 3, "BMX" });

            migrationBuilder.InsertData(
                table: "PricingCategories",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 4, "Детский" });

            migrationBuilder.InsertData(
                table: "PricingCategories",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 5, "Электро" });

            migrationBuilder.InsertData(
                table: "PricingCategories",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 6, "Аксессуар" });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 5, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, true, 1, 100.0, null, "MTB льготный час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 33, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, false, 1, 2000.0, null, "Ремонт колеса", 2 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 34, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, false, 1, 2000.0, null, "Ремонт цепи", 2 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 35, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, false, 1, 1000.0, null, "Ремонт тормоза", 2 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 1, false, null, 1, "black", "Горный велосипед", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 3, "GT AVALANCHE 27 SPORT", "27''", null });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 2, false, null, null, "aqua", "Горный велосипед", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 3, "GT AVALANCHE 27 SPORT", "27''", 1 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 3, false, null, 1, "black", "Горный велосипед", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 2, "GT AVALANCHE 27 SPORT", "27''", 1 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 4, false, null, 1, "aqua", "Горный велосипед", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 2, "GT AVALANCHE 27 SPORT", "27''", 1 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 5, false, null, 1, "black", "Горный велосипед", "https://trial-sport.ru/goods/51516/2541638.html", null, null, 4, "GT AVALANCHE 29 SPORT", "29''", 1 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 6, false, null, 1, "aqua", "Горный велосипед", "https://trial-sport.ru/goods/51516/2541638.html", null, null, 4, "GT AVALANCHE 29 SPORT", "29''", 1 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 7, false, null, 1, "navy", "Горный велосипед женский", "https://trial-sport.ru/goods/51516/1496760.html", 2, null, 1, "Mongoose SWITCHBACK SPORT W", "27.5''", 1 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 8, false, null, 1, "navy", "Горный велосипед женский", "https://trial-sport.ru/goods/51516/1496760.html", 2, null, 2, "Mongoose SWITCHBACK SPORT W", "27.5''", 1 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 9, false, 1, 2, "red", "Горный велосипед подростковый", "https://trial-sport.ru/goods/51516/1493214.html", null, null, 2, "Mongoose ROCKADILE 20", "20''", 2 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 10, false, 1, 2, "purple", "Горный велосипед подростковый", "https://trial-sport.ru/goods/51516/1496770.html", 2, null, 2, "Mongoose ROCKADILE 20 W", "20''", 2 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 11, false, null, 3, "black", "BMX", "https://trial-sport.ru/goods/51516/2033001.html", null, null, null, "Radio SAIKO 20", "20''", 3 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 12, false, null, 3, "metallic purple", "BMX", "https://trial-sport.ru/goods/51516/2033001.html", null, null, null, "Radio SAIKO 20", "20''", 3 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 13, false, null, 3, "matt black", "BMX", "https://trial-sport.ru/goods/51516/2033004.html", null, null, null, "Radio DARKO", "20''", 3 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 14, false, null, 3, "matt black", "BMX", "https://trial-sport.ru/goods/51516/2032941.html", null, null, null, "WeThePeople ARCADE", "20''", 3 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 15, false, 2, 4, "yellow", "Беговел", "https://trial-sport.ru/goods/51516/1493326.html", null, null, null, "Outleap ROCKET", "12''", 4 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 16, false, 2, 4, "black", "Беговел", "https://trial-sport.ru/goods/51516/1493326.html", null, null, null, "Outleap ROCKET", "12''", 4 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 17, false, 2, 4, "orange", "Беговел", "https://trial-sport.ru/goods/51516/1411943.html", null, null, null, "Outleap ROCKET", "12''", 4 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 18, false, 2, 4, "blue", "Беговел", "https://trial-sport.ru/goods/51516/1411943.html", null, null, null, "Outleap ROCKET", "12''", 4 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 19, false, null, 5, null, "Электровелосипед", "", null, null, null, "Himo C26", "26''", 5 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 20, false, null, 6, null, "Шлем", "", null, null, 3, "Шлем", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 21, false, null, 6, null, "Шлем", "", null, null, 2, "Шлем", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 22, false, null, 6, null, "Шлем", "", null, null, 1, "Шлем", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 23, false, 2, 6, null, "Шлем детский", "", null, null, null, "Шлем", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 24, false, null, 6, null, "Наколенники", "", null, null, 3, "Наколенники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 25, false, null, 6, null, "Наколенники", "", null, null, 2, "Наколенники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 26, false, null, 6, null, "Наколенники", "", null, null, 1, "Наколенники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 27, false, 2, 6, null, "Наколенники детские", "", null, null, null, "Наколенники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 28, false, null, 6, null, "Налокотники", "", null, null, 3, "Налокотники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 29, false, null, 6, null, "Налокотники", "", null, null, 2, "Налокотники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 30, false, null, 6, null, "Налокотники", "", null, null, 1, "Налокотники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 31, false, 2, 6, null, "Налокотники детские", "", null, null, null, "Налокотники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 32, false, null, 6, null, "Перчатки", "", null, null, 3, "Перчатки", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 33, false, null, 6, null, "Перчатки", "", null, null, 2, "Перчатки", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 34, false, null, 6, null, "Перчатки", "", null, null, 1, "Перчатки", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 35, false, 2, 6, null, "Перчатки детские", "", null, null, null, "Перчатки", null, 6 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 1, false, "Monday,Tuesday,Wednesday,Thursday", false, false, 1, 150.0, 1, "MTB будний час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 2, false, "Friday", false, false, 1, 150.0, 1, "MTB пятница час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 3, false, "Saturday,Sunday", false, false, 1, 200.0, 1, "MTB выходной час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 4, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", true, false, 1, 200.0, 1, "MTB праздник час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 6, false, "Monday,Tuesday,Wednesday,Thursday", false, false, 1, 800.0, 1, "MTB будний день", 0 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 7, false, "Friday", false, false, 1, 1000.0, 1, "MTB пятница день", 0 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 8, false, "Saturday,Sunday", false, false, 1, 1000.0, 1, "MTB выходной день", 0 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 9, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", true, false, 1, 1000.0, 1, "MTB праздник день", 0 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 10, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, true, 1, 400.0, 1, "MTB льготный день", 0 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 11, false, "Monday,Tuesday,Wednesday,Thursday", false, false, 1, 100.0, 2, "MTB подросток будний час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 12, false, "Friday", false, false, 1, 100.0, 2, "MTB подросток пятница час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 13, false, "Saturday,Sunday", false, false, 1, 150.0, 2, "MTB подросток выходной час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 14, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", true, false, 1, 150.0, 2, "MTB подросток праздник час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 15, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, true, 1, 50.0, 2, "MTB подросток льготный час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 16, false, "Monday,Tuesday,Wednesday,Thursday", false, false, 1, 500.0, 2, "MTB подросток будний день", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 17, false, "Friday", false, false, 1, 600.0, 2, "MTB подросток пятница день", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 18, false, "Saturday,Sunday", false, false, 1, 700.0, 2, "MTB подросток выходной день", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 19, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", true, false, 1, 700.0, 2, "MTB подросток праздник день", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 20, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, true, 1, 300.0, 2, "MTB подросток льготный день", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 21, false, "Monday,Tuesday,Wednesday,Thursday", false, false, 1, 100.0, 3, "BMX будний час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 22, false, "Friday,Saturday,Sunday", false, false, 1, 150.0, 3, "BMX пт-сб-вс час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 23, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", true, false, 1, 150.0, 3, "BMX праздник час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 24, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, false, 1, 100.0, 4, "Беговел час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 25, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, false, 1, 300.0, 5, "Электро час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 26, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, false, 1, 50.0, 6, "Аксессуар час", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 27, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, false, 1, 300.0, 6, "Аксессуар день", 0 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 28, false, "Monday,Tuesday,Wednesday,Thursday", false, false, 2, 125.0, 1, "MTB будний 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 29, false, "Friday", false, false, 2, 125.0, 1, "MTB пятница 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 30, false, "Saturday,Sunday", false, false, 2, 150.0, 1, "MTB выходной 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 31, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", true, false, 2, 150.0, 1, "MTB праздник 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[] { 32, false, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", false, true, 2, 75.0, 1, "MTB льготный 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "RecordID", "CustomInformation", "CustomerID", "End", "Price", "Start", "Status", "UserID" },
                values: new object[] { 1, null, 1, new DateTime(2022, 4, 19, 12, 0, 0, 0, DateTimeKind.Unspecified), 1200.0, new DateTime(2022, 4, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), 3, null });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "RecordID", "CustomInformation", "CustomerID", "End", "Price", "Start", "Status", "UserID" },
                values: new object[] { 2, null, 2, new DateTime(2022, 5, 15, 19, 0, 0, 0, DateTimeKind.Unspecified), 750.0, new DateTime(2022, 5, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), 1, null });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "RecordID", "CustomInformation", "CustomerID", "End", "Price", "Start", "Status", "UserID" },
                values: new object[] { 3, null, 1, new DateTime(2022, 5, 15, 19, 0, 0, 0, DateTimeKind.Unspecified), 750.0, new DateTime(2022, 5, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), 1, null });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "RecordID", "CustomInformation", "CustomerID", "End", "Price", "Start", "Status", "UserID" },
                values: new object[] { 4, null, 1, new DateTime(2022, 5, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), 1200.0, new DateTime(2022, 5, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, null });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "RecordID", "CustomInformation", "CustomerID", "End", "Price", "Start", "Status", "UserID" },
                values: new object[] { 5, null, 2, new DateTime(2022, 5, 14, 23, 0, 0, 0, DateTimeKind.Unspecified), 3000.0, new DateTime(2022, 5, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, null });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 1, false, "101", 1 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 2, false, "102", 1 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 3, false, "103", 2 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 4, false, "104", 2 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 5, false, "105", 3 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 6, false, "106", 3 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 7, false, "107", 4 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 8, false, "108", 4 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 9, false, "109", 5 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 10, false, "110", 5 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 11, false, "111", 5 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 12, false, "112", 6 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 13, false, "113", 7 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 14, false, "114", 8 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 15, false, "115", 8 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 16, false, "116", 9 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 17, false, "117", 9 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 18, false, "118", 10 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 19, false, "201", 11 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 20, false, "202", 12 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 21, false, "203", 13 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 22, false, "204", 13 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 23, false, "205", 14 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 24, false, "206", 14 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 25, false, "301", 15 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 26, false, "302", 16 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 27, false, "303", 17 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 28, false, "304", 18 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 29, false, "401", 19 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 30, false, "402", 19 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 31, false, "403", 19 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 32, false, "404", 19 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 33, false, "405", 19 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 34, false, "1", 20 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 35, false, "2", 21 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 36, false, "3", 21 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 37, false, "4", 22 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 38, false, "5", 23 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 39, false, "6", 23 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 40, false, "7", 24 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 41, false, "8", 25 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 42, false, "9", 25 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 43, false, "10", 26 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 44, false, "11", 27 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 45, false, "12", 27 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 46, false, "13", 28 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 47, false, "14", 29 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 48, false, "15", 29 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 49, false, "16", 30 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 50, false, "17", 31 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 51, false, "18", 31 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 52, false, "19", 32 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 53, false, "20", 33 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 54, false, "21", 33 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 55, false, "22", 34 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 56, false, "23", 35 });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[] { 57, false, "24", 35 });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 1, null, new DateTime(2022, 4, 19, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, new DateTime(2022, 4, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), 3, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 2, null, new DateTime(2022, 4, 19, 11, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 1, new DateTime(2022, 4, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), 3, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 3, null, new DateTime(2022, 4, 19, 12, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, 1, new DateTime(2022, 4, 19, 10, 0, 0, 0, DateTimeKind.Unspecified), 3, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 4, null, new DateTime(2022, 4, 19, 11, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, 1, new DateTime(2022, 4, 19, 10, 0, 0, 0, DateTimeKind.Unspecified), 3, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 5, null, new DateTime(2022, 4, 19, 12, 0, 0, 0, DateTimeKind.Unspecified), 34, 26, 1, new DateTime(2022, 4, 19, 9, 0, 0, 0, DateTimeKind.Unspecified), 3, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 6, null, new DateTime(2022, 5, 15, 19, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 2, new DateTime(2022, 5, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), 1, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 7, null, new DateTime(2022, 5, 15, 19, 0, 0, 0, DateTimeKind.Unspecified), 34, 26, 2, new DateTime(2022, 5, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), 1, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 8, null, new DateTime(2022, 5, 15, 19, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 3, new DateTime(2022, 5, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), 1, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 9, null, new DateTime(2022, 5, 15, 19, 0, 0, 0, DateTimeKind.Unspecified), 35, 26, 3, new DateTime(2022, 5, 15, 16, 0, 0, 0, DateTimeKind.Unspecified), 1, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 10, null, new DateTime(2022, 5, 16, 14, 0, 0, 0, DateTimeKind.Unspecified), 29, 25, 4, new DateTime(2022, 5, 16, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 11, null, new DateTime(2022, 5, 14, 23, 0, 0, 0, DateTimeKind.Unspecified), 29, 25, 5, new DateTime(2022, 5, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), 2, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 12, null, new DateTime(2022, 5, 14, 19, 0, 0, 0, DateTimeKind.Unspecified), 30, 25, 5, new DateTime(2022, 5, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), 3, null });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[] { 13, null, new DateTime(2022, 5, 14, 23, 0, 0, 0, DateTimeKind.Unspecified), 31, 25, 5, new DateTime(2022, 5, 14, 19, 0, 0, 0, DateTimeKind.Unspecified), 2, null });

            migrationBuilder.InsertData(
                table: "Prepared",
                columns: new[] { "ItemPreparedID", "ItemID" },
                values: new object[] { 1, 10 });

            migrationBuilder.InsertData(
                table: "Prepared",
                columns: new[] { "ItemPreparedID", "ItemID" },
                values: new object[] { 2, 11 });

            migrationBuilder.InsertData(
                table: "Prepared",
                columns: new[] { "ItemPreparedID", "ItemID" },
                values: new object[] { 3, 31 });

            migrationBuilder.InsertData(
                table: "Prepared",
                columns: new[] { "ItemPreparedID", "ItemID" },
                values: new object[] { 4, 32 });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemRecords_ItemID",
                table: "ItemRecords",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemRecords_PricingID",
                table: "ItemRecords",
                column: "PricingID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemRecords_RecordID",
                table: "ItemRecords",
                column: "RecordID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemRecords_UserID",
                table: "ItemRecords",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemID",
                table: "Items",
                column: "ItemID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeID",
                table: "Items",
                column: "ItemTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_ItemCategoryID",
                table: "ItemTypes",
                column: "ItemCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_PricingCategoryID",
                table: "ItemTypes",
                column: "PricingCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Prepared_ItemID",
                table: "Prepared",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Pricings_PricingCategoryID",
                table: "Pricings",
                column: "PricingCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Records_CustomerID",
                table: "Records",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Records_UserID",
                table: "Records",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "ItemRecords");

            migrationBuilder.DropTable(
                name: "Prepared");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Pricings");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "ItemCategories");

            migrationBuilder.DropTable(
                name: "PricingCategories");
        }
    }
}
