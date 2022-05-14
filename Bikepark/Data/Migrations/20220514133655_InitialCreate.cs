using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Data.Migrations
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
                name: "Customer",
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
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategory",
                columns: table => new
                {
                    ItemCategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemCategoryName = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategory", x => x.ItemCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "PricingCategory",
                columns: table => new
                {
                    PricingCategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PricingCategoryName = table.Column<string>(type: "nvarchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingCategory", x => x.PricingCategoryID);
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
                name: "RentalLog",
                columns: table => new
                {
                    RentalRecordID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerID = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomInformation = table.Column<string>(type: "TEXT", nullable: true),
                    RentalStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    RentalType = table.Column<int>(type: "INTEGER", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: true),
                    End = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndActual = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalLog", x => x.RentalRecordID);
                    table.ForeignKey(
                        name: "FK_RentalLog_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "ItemType",
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
                    ItemImageURL = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemType", x => x.ItemTypeID);
                    table.ForeignKey(
                        name: "FK_ItemType_ItemCategory_ItemCategoryID",
                        column: x => x.ItemCategoryID,
                        principalTable: "ItemCategory",
                        principalColumn: "ItemCategoryID");
                    table.ForeignKey(
                        name: "FK_ItemType_PricingCategory_PricingCategoryID",
                        column: x => x.PricingCategoryID,
                        principalTable: "PricingCategory",
                        principalColumn: "PricingCategoryID");
                });

            migrationBuilder.CreateTable(
                name: "RentalPricing",
                columns: table => new
                {
                    RentalPricingID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RentalPricingName = table.Column<string>(type: "TEXT", nullable: true),
                    PricingCategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    RentalType = table.Column<int>(type: "INTEGER", nullable: false),
                    DaysOfWeek = table.Column<string>(type: "TEXT", nullable: false),
                    IsHoliday = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsReduced = table.Column<bool>(type: "INTEGER", nullable: false),
                    MinDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    ExtraPrice = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPricing", x => x.RentalPricingID);
                    table.ForeignKey(
                        name: "FK_RentalPricing_PricingCategory_PricingCategoryID",
                        column: x => x.PricingCategoryID,
                        principalTable: "PricingCategory",
                        principalColumn: "PricingCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemTypeID = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemNumber = table.Column<string>(type: "TEXT", nullable: false),
                    ItemStatus = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Storage_ItemType_ItemTypeID",
                        column: x => x.ItemTypeID,
                        principalTable: "ItemType",
                        principalColumn: "ItemTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RentalItem",
                columns: table => new
                {
                    RentalItemID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RentalRecordID = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemID = table.Column<int>(type: "INTEGER", nullable: true),
                    RentalPricingID = table.Column<int>(type: "INTEGER", nullable: true),
                    IsPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    RentalStatus = table.Column<int>(type: "INTEGER", nullable: true),
                    RentalType = table.Column<int>(type: "INTEGER", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: true),
                    End = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EndActual = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalItem", x => x.RentalItemID);
                    table.ForeignKey(
                        name: "FK_RentalItem_RentalLog_RentalRecordID",
                        column: x => x.RentalRecordID,
                        principalTable: "RentalLog",
                        principalColumn: "RentalRecordID");
                    table.ForeignKey(
                        name: "FK_RentalItem_RentalPricing_RentalPricingID",
                        column: x => x.RentalPricingID,
                        principalTable: "RentalPricing",
                        principalColumn: "RentalPricingID");
                    table.ForeignKey(
                        name: "FK_RentalItem_Storage_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Storage",
                        principalColumn: "ItemID");
                });

            migrationBuilder.CreateTable(
                name: "ServiceLog",
                columns: table => new
                {
                    ServiceRecordID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemID = table.Column<int>(type: "INTEGER", nullable: true),
                    ServiceStartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ServiceEndTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RentalRecordID = table.Column<int>(type: "INTEGER", nullable: true),
                    Maintenance = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLog", x => x.ServiceRecordID);
                    table.ForeignKey(
                        name: "FK_ServiceLog_RentalLog_RentalRecordID",
                        column: x => x.RentalRecordID,
                        principalTable: "RentalLog",
                        principalColumn: "RentalRecordID");
                    table.ForeignKey(
                        name: "FK_ServiceLog_Storage_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Storage",
                        principalColumn: "ItemID");
                });

            migrationBuilder.CreateTable(
                name: "PaymentRecord",
                columns: table => new
                {
                    PaymentRecordID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Value = table.Column<double>(type: "REAL", nullable: true),
                    Executed = table.Column<bool>(type: "INTEGER", nullable: false),
                    RentalRecordID = table.Column<int>(type: "INTEGER", nullable: true),
                    ServiceRecordID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRecord", x => x.PaymentRecordID);
                    table.ForeignKey(
                        name: "FK_PaymentRecord_RentalLog_RentalRecordID",
                        column: x => x.RentalRecordID,
                        principalTable: "RentalLog",
                        principalColumn: "RentalRecordID");
                    table.ForeignKey(
                        name: "FK_PaymentRecord_ServiceLog_ServiceRecordID",
                        column: x => x.ServiceRecordID,
                        principalTable: "ServiceLog",
                        principalColumn: "ServiceRecordID");
                });

            migrationBuilder.CreateTable(
                name: "ServiceFee",
                columns: table => new
                {
                    ServiceFeeID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceFeeName = table.Column<string>(type: "TEXT", nullable: true),
                    ItemCategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    Fee = table.Column<double>(type: "REAL", nullable: false),
                    ServiceRecordID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFee", x => x.ServiceFeeID);
                    table.ForeignKey(
                        name: "FK_ServiceFee_ItemCategory_ItemCategoryID",
                        column: x => x.ItemCategoryID,
                        principalTable: "ItemCategory",
                        principalColumn: "ItemCategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceFee_ServiceLog_ServiceRecordID",
                        column: x => x.ServiceRecordID,
                        principalTable: "ServiceLog",
                        principalColumn: "ServiceRecordID");
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "CustomerID", "CustomerContactNumber", "CustomerEMail", "CustomerFullName", "CustomerPassport" },
                values: new object[] { 1, "+79781234567", "vasily.pupkin@maily.su", "Василий Пупкин", "00 000001" });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "CustomerID", "CustomerContactNumber", "CustomerEMail", "CustomerFullName", "CustomerPassport" },
                values: new object[] { 2, "+79780123456", "ivan.petrov@maily.su", "Иван Петров", "00 000002" });

            migrationBuilder.InsertData(
                table: "ItemCategory",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 1, "MTB" });

            migrationBuilder.InsertData(
                table: "ItemCategory",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 2, "MTB подростковый" });

            migrationBuilder.InsertData(
                table: "ItemCategory",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 3, "BMX" });

            migrationBuilder.InsertData(
                table: "ItemCategory",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 4, "Беговел" });

            migrationBuilder.InsertData(
                table: "ItemCategory",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 5, "Электровелосипед" });

            migrationBuilder.InsertData(
                table: "ItemCategory",
                columns: new[] { "ItemCategoryID", "ItemCategoryName" },
                values: new object[] { 6, "Аксессуар" });

            migrationBuilder.InsertData(
                table: "PricingCategory",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 1, "Горный" });

            migrationBuilder.InsertData(
                table: "PricingCategory",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 2, "Подросток" });

            migrationBuilder.InsertData(
                table: "PricingCategory",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 3, "BMX" });

            migrationBuilder.InsertData(
                table: "PricingCategory",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 4, "Детский" });

            migrationBuilder.InsertData(
                table: "PricingCategory",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 5, "Электро" });

            migrationBuilder.InsertData(
                table: "PricingCategory",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[] { 6, "Аксессуар" });

            migrationBuilder.InsertData(
                table: "RentalLog",
                columns: new[] { "RentalRecordID", "CustomInformation", "CustomerID", "End", "EndActual", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 6, null, null, null, null, null, 0, null });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 1, null, 1, "black", "Горный велосипед", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 3, "GT AVALANCHE 27 SPORT", "27''", 1 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 2, null, 1, "aqua", "Горный велосипед", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 3, "GT AVALANCHE 27 SPORT", "27''", 1 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 3, null, 1, "black", "Горный велосипед", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 2, "GT AVALANCHE 27 SPORT", "27''", 1 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 4, null, 1, "aqua", "Горный велосипед", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 2, "GT AVALANCHE 27 SPORT", "27''", 1 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 5, null, 1, "black", "Горный велосипед", "https://trial-sport.ru/goods/51516/2541638.html", null, null, 4, "GT AVALANCHE 29 SPORT", "29''", 1 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 6, null, 1, "aqua", "Горный велосипед", "https://trial-sport.ru/goods/51516/2541638.html", null, null, 4, "GT AVALANCHE 29 SPORT", "29''", 1 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 7, null, 1, "navy", "Горный велосипед женский", "https://trial-sport.ru/goods/51516/1496760.html", 2, null, 1, "Mongoose SWITCHBACK SPORT W", "27.5''", 1 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 8, null, 1, "navy", "Горный велосипед женский", "https://trial-sport.ru/goods/51516/1496760.html", 2, null, 2, "Mongoose SWITCHBACK SPORT W", "27.5''", 1 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 9, 1, 2, "red", "Горный велосипед подростковый", "https://trial-sport.ru/goods/51516/1493214.html", null, null, 2, "Mongoose ROCKADILE 20", "20''", 2 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 10, 1, 2, "purple", "Горный велосипед подростковый", "https://trial-sport.ru/goods/51516/1496770.html", 2, null, 2, "Mongoose ROCKADILE 20 W", "20''", 2 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 11, null, 3, "black", "BMX", "https://trial-sport.ru/goods/51516/2033001.html", null, null, null, "Radio SAIKO 20", "20''", 3 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 12, null, 3, "metallic purple", "BMX", "https://trial-sport.ru/goods/51516/2033001.html", null, null, null, "Radio SAIKO 20", "20''", 3 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 13, null, 3, "matt black", "BMX", "https://trial-sport.ru/goods/51516/2033004.html", null, null, null, "Radio DARKO", "20''", 3 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 14, null, 3, "matt black", "BMX", "https://trial-sport.ru/goods/51516/2032941.html", null, null, null, "WeThePeople ARCADE", "20''", 3 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 15, 2, 4, "yellow", "Беговел", "https://trial-sport.ru/goods/51516/1493326.html", null, null, null, "Outleap ROCKET", "12''", 4 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 16, 2, 4, "black", "Беговел", "https://trial-sport.ru/goods/51516/1493326.html", null, null, null, "Outleap ROCKET", "12''", 4 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 17, 2, 4, "orange", "Беговел", "https://trial-sport.ru/goods/51516/1411943.html", null, null, null, "Outleap ROCKET", "12''", 4 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 18, 2, 4, "blue", "Беговел", "https://trial-sport.ru/goods/51516/1411943.html", null, null, null, "Outleap ROCKET", "12''", 4 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 19, null, 5, null, "Электровелосипед", "", null, null, null, "Himo C26", "26''", 5 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 20, null, 6, null, "Шлем", "", null, null, 3, "Шлем", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 21, null, 6, null, "Шлем", "", null, null, 2, "Шлем", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 22, null, 6, null, "Шлем", "", null, null, 1, "Шлем", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 23, 2, 6, null, "Шлем детский", "", null, null, null, "Шлем", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 24, null, 6, null, "Наколенники", "", null, null, 3, "Наколенники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 25, null, 6, null, "Наколенники", "", null, null, 2, "Наколенники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 26, null, 6, null, "Наколенники", "", null, null, 1, "Наколенники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 27, 2, 6, null, "Наколенники детские", "", null, null, null, "Наколенники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 28, null, 6, null, "Налокотники", "", null, null, 3, "Налокотники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 29, null, 6, null, "Налокотники", "", null, null, 2, "Налокотники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 30, null, 6, null, "Налокотники", "", null, null, 1, "Налокотники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 31, 2, 6, null, "Налокотники детские", "", null, null, null, "Налокотники", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 32, null, 6, null, "Перчатки", "", null, null, 3, "Перчатки", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 33, null, 6, null, "Перчатки", "", null, null, 2, "Перчатки", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 34, null, 6, null, "Перчатки", "", null, null, 1, "Перчатки", null, 6 });

            migrationBuilder.InsertData(
                table: "ItemType",
                columns: new[] { "ItemTypeID", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[] { 35, 2, 6, null, "Перчатки детские", "", null, null, null, "Перчатки", null, 6 });

            migrationBuilder.InsertData(
                table: "RentalLog",
                columns: new[] { "RentalRecordID", "CustomInformation", "CustomerID", "End", "EndActual", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 1, null, 1, new DateTime(2022, 4, 19, 12, 0, 0, 0, DateTimeKind.Unspecified), null, 2, 1, new DateTime(2022, 4, 19, 9, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "RentalLog",
                columns: new[] { "RentalRecordID", "CustomInformation", "CustomerID", "End", "EndActual", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 2, null, 2, new DateTime(2022, 4, 30, 19, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 1, new DateTime(2022, 4, 30, 16, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "RentalLog",
                columns: new[] { "RentalRecordID", "CustomInformation", "CustomerID", "End", "EndActual", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 3, null, 1, new DateTime(2022, 4, 30, 19, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 1, new DateTime(2022, 4, 30, 16, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "RentalLog",
                columns: new[] { "RentalRecordID", "CustomInformation", "CustomerID", "End", "EndActual", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 4, null, 1, new DateTime(2022, 5, 1, 14, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 1, new DateTime(2022, 5, 1, 10, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "RentalLog",
                columns: new[] { "RentalRecordID", "CustomInformation", "CustomerID", "End", "EndActual", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 5, null, 2, new DateTime(2022, 5, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 2, new DateTime(2022, 4, 30, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 1, "Monday,Tuesday,Wednesday,Thursday", 1.0, false, false, 1, 150.0, 1, "MTB будний час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 2, "Friday", 1.0, false, false, 1, 150.0, 1, "MTB пятница час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 3, "Saturday,Sunday", 1.0, false, false, 1, 200.0, 1, "MTB выходной час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 4, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, true, false, 1, 200.0, 1, "MTB праздник час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 5, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, false, true, 1, 100.0, 1, "MTB льготный час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 6, "Monday,Tuesday,Wednesday,Thursday", 1.0, false, false, 1, 800.0, 1, "MTB будний день", 2 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 7, "Friday", 1.0, false, false, 1, 1000.0, 1, "MTB пятница день", 2 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 8, "Saturday,Sunday", 1.0, false, false, 1, 1000.0, 1, "MTB выходной день", 2 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 9, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, true, false, 1, 1000.0, 1, "MTB праздник день", 2 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 10, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, false, true, 1, 400.0, 1, "MTB льготный день", 2 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 11, "Monday,Tuesday,Wednesday,Thursday", 1.0, false, false, 1, 100.0, 2, "MTB подросток будний час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 12, "Friday", 1.0, false, false, 1, 100.0, 2, "MTB подросток пятница час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 13, "Saturday,Sunday", 1.0, false, false, 1, 150.0, 2, "MTB подросток выходной час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 14, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, true, false, 1, 150.0, 2, "MTB подросток праздник час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 15, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, false, true, 1, 50.0, 2, "MTB подросток льготный час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 16, "Monday,Tuesday,Wednesday,Thursday", 1.0, false, false, 1, 500.0, 2, "MTB подросток будний день", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 17, "Friday", 1.0, false, false, 1, 600.0, 2, "MTB подросток пятница день", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 18, "Saturday,Sunday", 1.0, false, false, 1, 700.0, 2, "MTB подросток выходной день", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 19, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, true, false, 1, 700.0, 2, "MTB подросток праздник день", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 20, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, false, true, 1, 300.0, 2, "MTB подросток льготный день", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 21, "Monday,Tuesday,Wednesday,Thursday", 1.0, false, false, 1, 100.0, 3, "BMX будний час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 22, "Friday,Saturday,Sunday", 1.0, false, false, 1, 150.0, 3, "BMX пт-сб-вс час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 23, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, true, false, 1, 150.0, 3, "BMX праздник час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 24, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, false, false, 1, 100.0, 4, "Беговел час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 25, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, false, false, 1, 300.0, 5, "Электро час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 26, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, false, false, 1, 50.0, 6, "Аксессуар час", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 27, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 1.0, false, false, 1, 300.0, 6, "Аксессуар день", 2 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 28, "Monday,Tuesday,Wednesday,Thursday", 150.0, false, false, 2, 125.0, 1, "MTB будний 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 29, "Friday", 150.0, false, false, 2, 125.0, 1, "MTB пятница 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 30, "Saturday,Sunday", 200.0, false, false, 2, 150.0, 1, "MTB выходной 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 31, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 200.0, true, false, 2, 150.0, 1, "MTB праздник 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "RentalPricing",
                columns: new[] { "RentalPricingID", "DaysOfWeek", "ExtraPrice", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "RentalPricingName", "RentalType" },
                values: new object[] { 32, "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday", 100.0, false, true, 2, 75.0, 1, "MTB льготный 2 часа", 1 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 1, "101", 1, 1 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 2, "102", 1, 1 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 3, "103", 0, 2 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 4, "104", 0, 2 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 5, "105", 0, 3 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 6, "106", 0, 3 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 7, "107", 0, 4 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 8, "108", 0, 4 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 9, "109", 0, 5 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 10, "110", 0, 5 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 11, "111", 0, 5 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 12, "112", 0, 6 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 13, "113", 0, 7 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 14, "114", 0, 8 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 15, "115", 0, 8 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 16, "116", 0, 9 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 17, "117", 0, 9 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 18, "118", 0, 10 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 19, "201", 0, 11 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 20, "202", 0, 12 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 21, "203", 0, 13 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 22, "204", 0, 13 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 23, "205", 0, 14 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 24, "206", 0, 14 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 25, "301", 0, 15 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 26, "302", 0, 16 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 27, "303", 0, 17 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 28, "304", 0, 18 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 29, "401", 2, 19 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 30, "402", 0, 19 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 31, "403", 0, 19 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 32, "404", 0, 19 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 33, "405", 0, 19 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 34, "1", 1, 20 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 35, "2", 1, 21 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 36, "3", 0, 21 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 37, "4", 0, 22 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 38, "5", 0, 23 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 39, "6", 0, 23 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 40, "7", 0, 24 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 41, "8", 0, 25 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 42, "9", 0, 25 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 43, "10", 0, 26 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 44, "11", 0, 27 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 45, "12", 0, 27 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 46, "13", 0, 28 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 47, "14", 0, 29 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 48, "15", 0, 29 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 49, "16", 0, 30 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 50, "17", 0, 31 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 51, "18", 0, 31 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 52, "19", 0, 32 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 53, "20", 0, 33 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 54, "21", 0, 33 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 55, "22", 0, 34 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 56, "23", 0, 35 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "ItemNumber", "ItemStatus", "ItemTypeID" },
                values: new object[] { 57, "24", 0, 35 });

            migrationBuilder.InsertData(
                table: "RentalItem",
                columns: new[] { "RentalItemID", "End", "EndActual", "IsPaid", "ItemID", "RentalPricingID", "RentalRecordID", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 1, null, null, false, 1, 1, 1, 2, 1, null });

            migrationBuilder.InsertData(
                table: "RentalItem",
                columns: new[] { "RentalItemID", "End", "EndActual", "IsPaid", "ItemID", "RentalPricingID", "RentalRecordID", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 2, null, null, false, 34, 18, 1, 2, 1, null });

            migrationBuilder.InsertData(
                table: "RentalItem",
                columns: new[] { "RentalItemID", "End", "EndActual", "IsPaid", "ItemID", "RentalPricingID", "RentalRecordID", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 3, null, null, false, 1, 1, 2, 0, 1, null });

            migrationBuilder.InsertData(
                table: "RentalItem",
                columns: new[] { "RentalItemID", "End", "EndActual", "IsPaid", "ItemID", "RentalPricingID", "RentalRecordID", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 4, null, null, false, 34, 18, 2, 0, 1, null });

            migrationBuilder.InsertData(
                table: "RentalItem",
                columns: new[] { "RentalItemID", "End", "EndActual", "IsPaid", "ItemID", "RentalPricingID", "RentalRecordID", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 5, null, null, false, 2, 1, 3, 0, 1, null });

            migrationBuilder.InsertData(
                table: "RentalItem",
                columns: new[] { "RentalItemID", "End", "EndActual", "IsPaid", "ItemID", "RentalPricingID", "RentalRecordID", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 6, null, null, false, 35, 18, 3, 0, 1, null });

            migrationBuilder.InsertData(
                table: "RentalItem",
                columns: new[] { "RentalItemID", "End", "EndActual", "IsPaid", "ItemID", "RentalPricingID", "RentalRecordID", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 7, null, null, false, 29, 17, 4, 0, 1, null });

            migrationBuilder.InsertData(
                table: "RentalItem",
                columns: new[] { "RentalItemID", "End", "EndActual", "IsPaid", "ItemID", "RentalPricingID", "RentalRecordID", "RentalStatus", "RentalType", "Start" },
                values: new object[] { 8, null, null, false, 29, 17, 5, 1, 1, null });

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
                name: "IX_ItemType_ItemCategoryID",
                table: "ItemType",
                column: "ItemCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ItemType_PricingCategoryID",
                table: "ItemType",
                column: "PricingCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecord_RentalRecordID",
                table: "PaymentRecord",
                column: "RentalRecordID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecord_ServiceRecordID",
                table: "PaymentRecord",
                column: "ServiceRecordID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalItem_ItemID",
                table: "RentalItem",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalItem_RentalPricingID",
                table: "RentalItem",
                column: "RentalPricingID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalItem_RentalRecordID",
                table: "RentalItem",
                column: "RentalRecordID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalLog_CustomerID",
                table: "RentalLog",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalPricing_PricingCategoryID",
                table: "RentalPricing",
                column: "PricingCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFee_ItemCategoryID",
                table: "ServiceFee",
                column: "ItemCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFee_ServiceRecordID",
                table: "ServiceFee",
                column: "ServiceRecordID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLog_ItemID",
                table: "ServiceLog",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLog_RentalRecordID",
                table: "ServiceLog",
                column: "RentalRecordID");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_ItemID",
                table: "Storage",
                column: "ItemID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Storage_ItemTypeID",
                table: "Storage",
                column: "ItemTypeID");
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
                name: "PaymentRecord");

            migrationBuilder.DropTable(
                name: "RentalItem");

            migrationBuilder.DropTable(
                name: "ServiceFee");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RentalPricing");

            migrationBuilder.DropTable(
                name: "ServiceLog");

            migrationBuilder.DropTable(
                name: "RentalLog");

            migrationBuilder.DropTable(
                name: "Storage");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ItemType");

            migrationBuilder.DropTable(
                name: "ItemCategory");

            migrationBuilder.DropTable(
                name: "PricingCategory");
        }
    }
}
