using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RenameIndustryColumnInStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.RenameColumn(
                    name: "Indutry",
                    table: "Stocks",
                    newName: "Industry");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameColumn(
                name: "Industry",
                table: "Stocks",
                newName: "Indutry");

        }
    }
}
