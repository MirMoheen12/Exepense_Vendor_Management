using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense_Vendor_Management.Migrations
{
    public partial class sdsdssd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "amount",
                table: "EmployeeExpense",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "amount",
                table: "EmployeeExpense",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
