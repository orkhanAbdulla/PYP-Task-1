using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelUploadReadDataSave.Persistence.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Segment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountBand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitsSold = table.Column<double>(type: "float", nullable: false),
                    ManufacturingPrice = table.Column<int>(type: "int", nullable: false),
                    SalePrice = table.Column<int>(type: "int", nullable: false),
                    GrossSales = table.Column<long>(type: "bigint", nullable: false),
                    Discounts = table.Column<double>(type: "float", nullable: false),
                    Sales = table.Column<double>(type: "float", nullable: false),
                    COGS = table.Column<int>(type: "int", nullable: false),
                    Profit = table.Column<double>(type: "float", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
