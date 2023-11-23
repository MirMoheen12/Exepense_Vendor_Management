using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense_Vendor_Management.Migrations
{
    public partial class kds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DlrAmt",
                table: "Vendor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "Vendor",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DlrAmt",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Vendor");
        }
    }
}
