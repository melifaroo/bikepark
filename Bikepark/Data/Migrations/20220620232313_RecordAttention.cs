using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bikepark.Data.Migrations
{
    public partial class RecordAttention : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Items_ItemID",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "AttentionStatus",
                table: "Records",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AttentionStatus",
                table: "ItemRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttentionStatus",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "AttentionStatus",
                table: "ItemRecords");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemID",
                table: "Items",
                column: "ItemID",
                unique: true);
        }
    }
}
