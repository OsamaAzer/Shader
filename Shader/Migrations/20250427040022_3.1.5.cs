using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class _315 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "CashTransactionFruits");

            migrationBuilder.AddColumn<decimal>(
                name: "NumberOfKilogramsSold",
                table: "Fruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceOfKilogramsSold",
                table: "Fruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfCages",
                table: "CashTransactionFruits",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfKilogramsSold",
                table: "Fruits");

            migrationBuilder.DropColumn(
                name: "PriceOfKilogramsSold",
                table: "Fruits");

            migrationBuilder.AlterColumn<decimal>(
                name: "NumberOfCages",
                table: "CashTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "CashTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
