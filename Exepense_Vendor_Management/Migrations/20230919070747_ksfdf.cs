using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense_Vendor_Management.Migrations
{
    public partial class ksfdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Vendor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Vendor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "Vendor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactWebsite",
                table: "Vendor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CustomerAccess",
                table: "Vendor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TechnolgyVendor",
                table: "Vendor",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "ContactWebsite",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "CustomerAccess",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "TechnolgyVendor",
                table: "Vendor");
        }
    }
}
