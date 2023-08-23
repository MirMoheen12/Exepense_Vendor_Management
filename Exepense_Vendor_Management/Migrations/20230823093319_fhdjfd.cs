using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exepense_Vendor_Management.Migrations
{
    public partial class fhdjfd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "supportingDocid",
                table: "EmployeeExpense");

            migrationBuilder.AlterColumn<string>(
                name: "modifiedBy",
                table: "EmployeeExpense",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "modifiedBy",
                table: "EmployeeExpense",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "supportingDocid",
                table: "EmployeeExpense",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
