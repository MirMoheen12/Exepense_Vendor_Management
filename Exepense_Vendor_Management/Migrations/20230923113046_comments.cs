using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense_Vendor_Management.Migrations
{
    public partial class comments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentsSection",
                columns: table => new
                {
                    CSID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Commentsby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false),
                    Comfor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentsDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsSection", x => x.CSID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentsSection");
        }
    }
}
