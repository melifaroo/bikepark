using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bikepark.Data.Migrations
{
    public partial class ItemCategoryAccesoriesField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Accessories",
                table: "ItemCategories",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ItemCategories",
                keyColumn: "ItemCategoryID",
                keyValue: 6,
                column: "Accessories",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accessories",
                table: "ItemCategories");
        }
    }
}
