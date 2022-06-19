using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bikepark.Data.Migrations
{
    public partial class MoreCustomerField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerPassport",
                table: "Customers",
                newName: "CustomerInformation");

            migrationBuilder.AddColumn<string>(
                name: "CustomerDocumentNumber",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerDocumentSeries",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerDocumentType",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 1,
                columns: new[] { "CustomerDocumentNumber", "CustomerInformation" },
                values: new object[] { "00 000001", null });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 2,
                columns: new[] { "CustomerDocumentNumber", "CustomerInformation" },
                values: new object[] { "00 000002", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerDocumentNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerDocumentSeries",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustomerDocumentType",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "CustomerInformation",
                table: "Customers",
                newName: "CustomerPassport");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 1,
                column: "CustomerPassport",
                value: "00 000001");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerID",
                keyValue: 2,
                column: "CustomerPassport",
                value: "00 000002");
        }
    }
}
