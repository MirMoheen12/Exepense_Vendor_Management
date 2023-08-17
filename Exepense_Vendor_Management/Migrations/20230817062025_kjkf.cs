using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exepense_Vendor_Management.Migrations
{
    public partial class kjkf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeExpense",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    submissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<float>(type: "real", nullable: false),
                    expenseOccurred = table.Column<DateTime>(type: "datetime2", nullable: false),
                    expenseCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vandorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supportingDocid = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeExpense", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeExpense");
        }
    }
}
