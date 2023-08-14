using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exepense_Vendor_Management.Migrations
{
    public partial class kh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    createdOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    createdBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vendorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    costCenter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    poductType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    catagory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    criticalVendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contractid = table.Column<int>(type: "int", nullable: false),
                    assesmentsid = table.Column<int>(type: "int", nullable: false),
                    otherDocsid = table.Column<int>(type: "int", nullable: false),
                    paymentAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    autoPayment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contractExpiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    autoRenew = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateToCancel = table.Column<DateTime>(type: "datetime2", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vendor");
        }
    }
}
