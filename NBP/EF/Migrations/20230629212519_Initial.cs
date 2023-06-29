using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBP.EF.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Table = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(30,8)", precision: 30, scale: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_CurrencyCode_Date_Table",
                table: "CurrencyRates",
                columns: new[] { "CurrencyCode", "Date", "Table" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRates");
        }
    }
}
