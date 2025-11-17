using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductIngredients");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "ProductIngredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "ProductIngredients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ProductIngredients");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ProductIngredients");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProductIngredients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
