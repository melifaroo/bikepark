using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bikepark.Data.Migrations
{
    public partial class RenameCustomerField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerContactNumber",
                table: "Customers",
                newName: "CustomerPhoneNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerPhoneNumber",
                table: "Customers",
                newName: "CustomerContactNumber");
        }
    }
}
