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
                name: "Categories",
                columns: table => new
                {
                    ItemCategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ItemCategoryID);
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
                name: "SubCategories",
                columns: table => new
                {
                    ItemSubCategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ItemCategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.ItemSubCategoryID);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_ItemCategoryID",
                        column: x => x.ItemCategoryID,
                        principalTable: "Categories",
                        principalColumn: "ItemCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    SubCategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Storage_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ItemCategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Storage_SubCategories_SubCategoryID",
                        column: x => x.SubCategoryID,
                        principalTable: "SubCategories",
                        principalColumn: "ItemSubCategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ItemCategoryID", "Name" },
                values: new object[] { 1, "Bike" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ItemCategoryID", "Name" },
                values: new object[] { 2, "Scooter" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ItemCategoryID", "Name" },
                values: new object[] { 3, "Accessories" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 1, 1, "MTB" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 2, 1, "BMX" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 3, 1, "eBike" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 4, 1, "BalanceBike" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 5, 2, "eScooter" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 6, 2, "KickScooter" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 7, 3, "Helmet" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 8, 3, "Paddings" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 9, 3, "Gloves" });

            migrationBuilder.InsertData(
                table: "SubCategories",
                columns: new[] { "ItemSubCategoryID", "ItemCategoryID", "Name" },
                values: new object[] { 10, 3, "Cam" });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "CategoryID", "Name", "Number", "Size", "Status", "SubCategoryID" },
                values: new object[] { 1, 1, "Mongoose", 1, 20, 0, 1 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "CategoryID", "Name", "Number", "Size", "Status", "SubCategoryID" },
                values: new object[] { 2, 1, "Mongoose", 2, 20, 0, 1 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "CategoryID", "Name", "Number", "Size", "Status", "SubCategoryID" },
                values: new object[] { 3, 1, "Mongoose", 3, 30, 0, 1 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "CategoryID", "Name", "Number", "Size", "Status", "SubCategoryID" },
                values: new object[] { 4, 1, "GT", 4, 40, 0, 1 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "CategoryID", "Name", "Number", "Size", "Status", "SubCategoryID" },
                values: new object[] { 5, 1, "GT", 5, 40, 0, 1 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "CategoryID", "Name", "Number", "Size", "Status", "SubCategoryID" },
                values: new object[] { 6, 1, "GT", 1, 0, 0, 2 });

            migrationBuilder.InsertData(
                table: "Storage",
                columns: new[] { "ItemID", "CategoryID", "Name", "Number", "Size", "Status", "SubCategoryID" },
                values: new object[] { 7, 1, "GT", 2, 0, 0, 2 });

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
                name: "IX_Storage_CategoryID",
                table: "Storage",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Storage_ItemID",
                table: "Storage",
                column: "ItemID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Storage_SubCategoryID",
                table: "Storage",
                column: "SubCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_ItemCategoryID",
                table: "SubCategories",
                column: "ItemCategoryID");
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
                name: "Storage");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
