using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Bikepark.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerPhoneNumber = table.Column<string>(type: "text", nullable: true),
                    CustomerFullName = table.Column<string>(type: "text", nullable: true),
                    CustomerDocumentType = table.Column<string>(type: "text", nullable: true),
                    CustomerDocumentSeries = table.Column<string>(type: "text", nullable: true),
                    CustomerDocumentNumber = table.Column<string>(type: "text", nullable: true),
                    CustomerEMail = table.Column<string>(type: "text", nullable: true),
                    CustomerInformation = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    HolidayID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.HolidayID);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    ItemCategoryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemCategoryName = table.Column<string>(type: "text", nullable: false),
                    Accessories = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => x.ItemCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "PricingCategories",
                columns: table => new
                {
                    PricingCategoryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PricingCategoryName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingCategories", x => x.PricingCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
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
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
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
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
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
                    RecordID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerID = table.Column<int>(type: "integer", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    End = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CustomInformation = table.Column<string>(type: "text", nullable: true),
                    UserID = table.Column<string>(type: "text", nullable: true),
                    AttentionStatus = table.Column<int>(type: "integer", nullable: false)
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
                    ItemTypeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemTypeName = table.Column<string>(type: "text", nullable: true),
                    ItemCategoryID = table.Column<int>(type: "integer", nullable: true),
                    PricingCategoryID = table.Column<int>(type: "integer", nullable: true),
                    ItemAge = table.Column<int>(type: "integer", nullable: true),
                    ItemGender = table.Column<int>(type: "integer", nullable: true),
                    ItemSize = table.Column<int>(type: "integer", nullable: true),
                    ItemWheelSize = table.Column<string>(type: "text", nullable: true),
                    ItemColor = table.Column<string>(type: "text", nullable: true),
                    ItemDescription = table.Column<string>(type: "text", nullable: true),
                    ItemExternalURL = table.Column<string>(type: "text", nullable: true),
                    ItemImageURL = table.Column<string>(type: "text", nullable: true),
                    Archival = table.Column<bool>(type: "boolean", nullable: false)
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
                    PricingID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PricingName = table.Column<string>(type: "text", nullable: true),
                    PricingCategoryID = table.Column<int>(type: "integer", nullable: true),
                    PricingType = table.Column<int>(type: "integer", nullable: false),
                    DaysOfWeek = table.Column<int>(type: "integer", nullable: false),
                    IsHoliday = table.Column<bool>(type: "boolean", nullable: false),
                    IsReduced = table.Column<bool>(type: "boolean", nullable: false),
                    MinDuration = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Archival = table.Column<bool>(type: "boolean", nullable: false)
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
                    ItemID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemTypeID = table.Column<int>(type: "integer", nullable: true),
                    ItemNumber = table.Column<string>(type: "text", nullable: false),
                    Archival = table.Column<bool>(type: "boolean", nullable: false)
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
                    ItemRecordID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RecordID = table.Column<int>(type: "integer", nullable: true),
                    ItemID = table.Column<int>(type: "integer", nullable: true),
                    PricingID = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    End = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CustomInformation = table.Column<string>(type: "text", nullable: true),
                    UserID = table.Column<string>(type: "text", nullable: true),
                    AttentionStatus = table.Column<int>(type: "integer", nullable: false)
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
                    ItemPreparedID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemID = table.Column<int>(type: "integer", nullable: false)
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
                columns: new[] { "CustomerID", "CustomerDocumentNumber", "CustomerDocumentSeries", "CustomerDocumentType", "CustomerEMail", "CustomerFullName", "CustomerInformation", "CustomerPhoneNumber" },
                values: new object[,]
                {
                    { 1, "00 000001", null, null, "john.doe@fake.domain", "John Doe", null, "05551234567" },
                    { 2, "00 000002", null, null, "jane.doe@fake.domain", "Jane Doe", null, "05550123456" }
                });

            migrationBuilder.InsertData(
                table: "Holidays",
                columns: new[] { "HolidayID", "Date", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Local), "Labor Day" },
                    { 2, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Local), "New Year's Eve" },
                    { 3, new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "New Year" },
                    { 4, new DateTime(2027, 5, 1, 0, 0, 0, 0, DateTimeKind.Local), "Labor Day" }
                });

            migrationBuilder.InsertData(
                table: "ItemCategories",
                columns: new[] { "ItemCategoryID", "Accessories", "ItemCategoryName" },
                values: new object[,]
                {
                    { 1, false, "MTB" },
                    { 2, false, "MTB Teenage" },
                    { 3, false, "BMX" },
                    { 4, false, "Balance" },
                    { 5, false, "Electric" },
                    { 6, true, "Accessory" }
                });

            migrationBuilder.InsertData(
                table: "PricingCategories",
                columns: new[] { "PricingCategoryID", "PricingCategoryName" },
                values: new object[,]
                {
                    { 1, "MTB" },
                    { 2, "Teenage" },
                    { 3, "BMX" },
                    { 4, "Child" },
                    { 5, "Electric" },
                    { 6, "Accessory" }
                });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[,]
                {
                    { 5, false, 127, false, true, 1, 100.0, null, "MTB - concessional - 1 hour", 1 },
                    { 33, false, 127, false, false, 1, 2000.0, null, "Tire Repair", 2 },
                    { 34, false, 127, false, false, 1, 2000.0, null, "Chain Repair", 2 },
                    { 35, false, 127, false, false, 1, 1000.0, null, "Brake Repair", 2 }
                });

            migrationBuilder.InsertData(
                table: "ItemTypes",
                columns: new[] { "ItemTypeID", "Archival", "ItemAge", "ItemCategoryID", "ItemColor", "ItemDescription", "ItemExternalURL", "ItemGender", "ItemImageURL", "ItemSize", "ItemTypeName", "ItemWheelSize", "PricingCategoryID" },
                values: new object[,]
                {
                    { 1, false, null, 1, "black", "Mountain Bike", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 3, "GT AVALANCHE 27 SPORT", "27''", null },
                    { 2, false, null, null, "aqua", "Mountain Bike", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 3, "GT AVALANCHE 27 SPORT", "27''", 1 },
                    { 3, false, null, 1, "black", "Mountain Bike", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 2, "GT AVALANCHE 27 SPORT", "27''", 1 },
                    { 4, false, null, 1, "aqua", "Mountain Bike", "https://trial-sport.ru/goods/51516/1490897.html", null, null, 2, "GT AVALANCHE 27 SPORT", "27''", 1 },
                    { 5, false, null, 1, "black", "Mountain Bike", "https://trial-sport.ru/goods/51516/2541638.html", null, null, 4, "GT AVALANCHE 29 SPORT", "29''", 1 },
                    { 6, false, null, 1, "aqua", "Mountain Bike", "https://trial-sport.ru/goods/51516/2541638.html", null, null, 4, "GT AVALANCHE 29 SPORT", "29''", 1 },
                    { 7, false, null, 1, "navy", "Mountain Bike, Women's", "https://trial-sport.ru/goods/51516/1496760.html", 2, null, 1, "Mongoose SWITCHBACK SPORT W", "27.5''", 1 },
                    { 8, false, null, 1, "navy", "Mountain Bike, Women's", "https://trial-sport.ru/goods/51516/1496760.html", 2, null, 2, "Mongoose SWITCHBACK SPORT W", "27.5''", 1 },
                    { 9, false, 1, 2, "red", "Mountain Bike, Teenage", "https://trial-sport.ru/goods/51516/1493214.html", null, null, 2, "Mongoose ROCKADILE 20", "20''", 2 },
                    { 10, false, 1, 2, "purple", "Mountain Bike, Teenage", "https://trial-sport.ru/goods/51516/1496770.html", 2, null, 2, "Mongoose ROCKADILE 20 W", "20''", 2 },
                    { 11, false, null, 3, "black", "BMX", "https://trial-sport.ru/goods/51516/2033001.html", null, null, null, "Radio SAIKO 20", "20''", 3 },
                    { 12, false, null, 3, "metallic purple", "BMX", "https://trial-sport.ru/goods/51516/2033001.html", null, null, null, "Radio SAIKO 20", "20''", 3 },
                    { 13, false, null, 3, "matt black", "BMX", "https://trial-sport.ru/goods/51516/2033004.html", null, null, null, "Radio DARKO", "20''", 3 },
                    { 14, false, null, 3, "matt black", "BMX", "https://trial-sport.ru/goods/51516/2032941.html", null, null, null, "WeThePeople ARCADE", "20''", 3 },
                    { 15, false, 2, 4, "yellow", "Balance Bike", "https://trial-sport.ru/goods/51516/1493326.html", null, null, null, "Outleap ROCKET", "12''", 4 },
                    { 16, false, 2, 4, "black", "Balance Bike", "https://trial-sport.ru/goods/51516/1493326.html", null, null, null, "Outleap ROCKET", "12''", 4 },
                    { 17, false, 2, 4, "orange", "Balance Bike", "https://trial-sport.ru/goods/51516/1411943.html", null, null, null, "Outleap ROCKET", "12''", 4 },
                    { 18, false, 2, 4, "blue", "Balance Bike", "https://trial-sport.ru/goods/51516/1411943.html", null, null, null, "Outleap ROCKET", "12''", 4 },
                    { 19, false, null, 5, null, "E-Bike, Electric Bike", "", null, null, null, "Himo C26", "26''", 5 },
                    { 20, false, null, 6, null, "Helmet", "", null, null, 3, "Helmet", null, 6 },
                    { 21, false, null, 6, null, "Helmet", "", null, null, 2, "Helmet", null, 6 },
                    { 22, false, null, 6, null, "Helmet", "", null, null, 1, "Helmet", null, 6 },
                    { 23, false, 2, 6, null, "Helmet, Children's", "", null, null, null, "Helmet", null, 6 },
                    { 24, false, null, 6, null, "Knee Pads", "", null, null, 3, "Knee Pads", null, 6 },
                    { 25, false, null, 6, null, "Knee Pads", "", null, null, 2, "Knee Pads", null, 6 },
                    { 26, false, null, 6, null, "Knee Pads", "", null, null, 1, "Knee Pads", null, 6 },
                    { 27, false, 2, 6, null, "Knee Pads, Children's", "", null, null, null, "Knee Pads", null, 6 },
                    { 28, false, null, 6, null, "Elbow Pads", "", null, null, 3, "Elbow Pads", null, 6 },
                    { 29, false, null, 6, null, "Elbow Pads", "", null, null, 2, "Elbow Pads", null, 6 },
                    { 30, false, null, 6, null, "Elbow Pads", "", null, null, 1, "Elbow Pads", null, 6 },
                    { 31, false, 2, 6, null, "Elbow Pads, Children's", "", null, null, null, "Elbow Pads", null, 6 },
                    { 32, false, null, 6, null, "Gloves", "", null, null, 3, "Gloves", null, 6 },
                    { 33, false, null, 6, null, "Gloves", "", null, null, 2, "Gloves", null, 6 },
                    { 34, false, null, 6, null, "Gloves", "", null, null, 1, "Gloves", null, 6 },
                    { 35, false, 2, 6, null, "Gloves, Children's", "", null, null, null, "Gloves", null, 6 }
                });

            migrationBuilder.InsertData(
                table: "Pricings",
                columns: new[] { "PricingID", "Archival", "DaysOfWeek", "IsHoliday", "IsReduced", "MinDuration", "Price", "PricingCategoryID", "PricingName", "PricingType" },
                values: new object[,]
                {
                    { 1, false, 15, false, false, 1, 150.0, 1, "MTB - weekday - 1 hour", 1 },
                    { 2, false, 16, false, false, 1, 150.0, 1, "MTB - friday - 1 hour", 1 },
                    { 3, false, 96, false, false, 1, 200.0, 1, "MTB - day-off - 1 hour", 1 },
                    { 4, false, 127, true, false, 1, 200.0, 1, "MTB - day-off - 1 hour", 1 },
                    { 6, false, 15, false, false, 1, 800.0, 1, "MTB - weekday - whole day (1 day)", 0 },
                    { 7, false, 16, false, false, 1, 1000.0, 1, "MTB - friday - whole day (1 day)", 0 },
                    { 8, false, 96, false, false, 1, 1000.0, 1, "MTB - day-off - whole day (1 day)", 0 },
                    { 9, false, 127, true, false, 1, 1000.0, 1, "MTB - holiday - whole day (1 day)", 0 },
                    { 10, false, 127, false, true, 1, 400.0, 1, "MTB - concessional - whole day (1 day)", 0 },
                    { 11, false, 15, false, false, 1, 100.0, 2, "MTB Teenager - weekday - 1 hour", 1 },
                    { 12, false, 16, false, false, 1, 100.0, 2, "MTB Teenager - friday - 1 hour", 1 },
                    { 13, false, 96, false, false, 1, 150.0, 2, "MTB Teenager - day-off - 1 hour", 1 },
                    { 14, false, 127, true, false, 1, 150.0, 2, "MTB Teenager - holiday - 1 hour", 1 },
                    { 15, false, 127, false, true, 1, 50.0, 2, "MTB Teenager - concessional - 1 hour", 1 },
                    { 16, false, 15, false, false, 1, 500.0, 2, "MTB Teenager - weekday - whole day (1 day)", 1 },
                    { 17, false, 16, false, false, 1, 600.0, 2, "MTB Teenager - friday - whole day (1 day)", 1 },
                    { 18, false, 96, false, false, 1, 700.0, 2, "MTB Teenager - day-off - whole day (1 day)", 1 },
                    { 19, false, 127, true, false, 1, 700.0, 2, "MTB Teenager - holiday - whole day (1 day)", 1 },
                    { 20, false, 127, false, true, 1, 300.0, 2, "MTB Teenager - concessional - whole day (1 day)", 1 },
                    { 21, false, 15, false, false, 1, 100.0, 3, "BMX - weekday - 1 hour", 1 },
                    { 22, false, 112, false, false, 1, 150.0, 3, "BMX - DaysOfWeekFlags.Weekend+Fri - 1 hour", 1 },
                    { 23, false, 127, true, false, 1, 150.0, 3, "BMX - holiday - 1 hour", 1 },
                    { 24, false, 127, false, false, 1, 100.0, 4, "Balance - 1 hour", 1 },
                    { 25, false, 127, false, false, 1, 300.0, 5, "Electro - 1 hour", 1 },
                    { 26, false, 127, false, false, 1, 50.0, 6, "Accessory - 1 hour", 1 },
                    { 27, false, 127, false, false, 1, 300.0, 6, "Accessory - whole day (1 day)", 0 },
                    { 28, false, 15, false, false, 2, 125.0, 1, "MTB - weekday - 2 hours", 1 },
                    { 29, false, 16, false, false, 2, 125.0, 1, "MTB - friday - 2 hours", 1 },
                    { 30, false, 96, false, false, 2, 150.0, 1, "MTB - day-off - 2 hours", 1 },
                    { 31, false, 127, true, false, 2, 150.0, 1, "MTB - holiday - 2 hours", 1 },
                    { 32, false, 127, false, true, 2, 75.0, 1, "MTB - concessional - 2 hours", 1 }
                });

            migrationBuilder.InsertData(
                table: "Records",
                columns: new[] { "RecordID", "AttentionStatus", "CustomInformation", "CustomerID", "End", "Price", "Start", "Status", "UserID" },
                values: new object[,]
                {
                    { 1, 0, null, 1, new DateTime(2026, 4, 30, 12, 0, 0, 0, DateTimeKind.Local), 1200.0, new DateTime(2026, 4, 30, 9, 0, 0, 0, DateTimeKind.Local), 3, null },
                    { 2, 0, null, 2, new DateTime(2026, 5, 31, 19, 0, 0, 0, DateTimeKind.Local), 750.0, new DateTime(2026, 5, 31, 16, 0, 0, 0, DateTimeKind.Local), 1, null },
                    { 3, 0, null, 2, new DateTime(2026, 5, 23, 19, 0, 0, 0, DateTimeKind.Local), 3000.0, new DateTime(2026, 5, 23, 9, 0, 0, 0, DateTimeKind.Local), 2, null }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemID", "Archival", "ItemNumber", "ItemTypeID" },
                values: new object[,]
                {
                    { 1, false, "101", 1 },
                    { 2, false, "102", 1 },
                    { 3, false, "103", 2 },
                    { 4, false, "104", 2 },
                    { 5, false, "105", 3 },
                    { 6, false, "106", 3 },
                    { 7, false, "107", 4 },
                    { 8, false, "108", 4 },
                    { 9, false, "109", 5 },
                    { 10, false, "110", 5 },
                    { 11, false, "111", 5 },
                    { 12, false, "112", 6 },
                    { 13, false, "113", 7 },
                    { 14, false, "114", 8 },
                    { 15, false, "115", 8 },
                    { 16, false, "116", 9 },
                    { 17, false, "117", 9 },
                    { 18, false, "118", 10 },
                    { 19, false, "201", 11 },
                    { 20, false, "202", 12 },
                    { 21, false, "203", 13 },
                    { 22, false, "204", 13 },
                    { 23, false, "205", 14 },
                    { 24, false, "206", 14 },
                    { 25, false, "301", 15 },
                    { 26, false, "302", 16 },
                    { 27, false, "303", 17 },
                    { 28, false, "304", 18 },
                    { 29, false, "401", 19 },
                    { 30, false, "402", 19 },
                    { 31, false, "403", 19 },
                    { 32, false, "404", 19 },
                    { 33, false, "405", 19 },
                    { 34, false, "1", 20 },
                    { 35, false, "2", 21 },
                    { 36, false, "3", 21 },
                    { 37, false, "4", 22 },
                    { 38, false, "5", 23 },
                    { 39, false, "6", 23 },
                    { 40, false, "7", 24 },
                    { 41, false, "8", 25 },
                    { 42, false, "9", 25 },
                    { 43, false, "10", 26 },
                    { 44, false, "11", 27 },
                    { 45, false, "12", 27 },
                    { 46, false, "13", 28 },
                    { 47, false, "14", 29 },
                    { 48, false, "15", 29 },
                    { 49, false, "16", 30 },
                    { 50, false, "17", 31 },
                    { 51, false, "18", 31 },
                    { 52, false, "19", 32 },
                    { 53, false, "20", 33 },
                    { 54, false, "21", 33 },
                    { 55, false, "22", 34 },
                    { 56, false, "23", 35 },
                    { 57, false, "24", 35 }
                });

            migrationBuilder.InsertData(
                table: "ItemRecords",
                columns: new[] { "ItemRecordID", "AttentionStatus", "CustomInformation", "End", "ItemID", "PricingID", "RecordID", "Start", "Status", "UserID" },
                values: new object[,]
                {
                    { 1, 0, null, new DateTime(2026, 4, 30, 10, 0, 0, 0, DateTimeKind.Local), 1, 1, 1, new DateTime(2026, 4, 30, 9, 0, 0, 0, DateTimeKind.Local), 3, null },
                    { 2, 0, null, new DateTime(2026, 4, 30, 10, 0, 0, 0, DateTimeKind.Local), 2, 1, 1, new DateTime(2026, 4, 30, 9, 0, 0, 0, DateTimeKind.Local), 3, null },
                    { 3, 0, null, new DateTime(2026, 4, 30, 12, 0, 0, 0, DateTimeKind.Local), 34, 26, 1, new DateTime(2026, 4, 30, 9, 0, 0, 0, DateTimeKind.Local), 3, null },
                    { 4, 0, null, new DateTime(2026, 4, 30, 12, 0, 0, 0, DateTimeKind.Local), 3, 1, 1, new DateTime(2026, 4, 30, 10, 0, 0, 0, DateTimeKind.Local), 3, null },
                    { 5, 0, null, new DateTime(2026, 4, 30, 12, 0, 0, 0, DateTimeKind.Local), 4, 1, 1, new DateTime(2026, 4, 30, 10, 0, 0, 0, DateTimeKind.Local), 3, null },
                    { 6, 0, null, new DateTime(2026, 5, 31, 19, 0, 0, 0, DateTimeKind.Local), 1, 1, 2, new DateTime(2026, 5, 31, 16, 0, 0, 0, DateTimeKind.Local), 1, null },
                    { 7, 0, null, new DateTime(2026, 5, 31, 19, 0, 0, 0, DateTimeKind.Local), 34, 26, 2, new DateTime(2026, 5, 31, 16, 0, 0, 0, DateTimeKind.Local), 1, null },
                    { 8, 0, null, new DateTime(2026, 5, 23, 19, 0, 0, 0, DateTimeKind.Local), 2, 1, 3, new DateTime(2026, 5, 23, 9, 0, 0, 0, DateTimeKind.Local), 2, null },
                    { 9, 0, null, new DateTime(2026, 5, 23, 19, 0, 0, 0, DateTimeKind.Local), 35, 26, 3, new DateTime(2026, 5, 23, 9, 0, 0, 0, DateTimeKind.Local), 2, null }
                });

            migrationBuilder.InsertData(
                table: "Prepared",
                columns: new[] { "ItemPreparedID", "ItemID" },
                values: new object[,]
                {
                    { 1, 10 },
                    { 2, 11 },
                    { 3, 31 },
                    { 4, 32 }
                });

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
