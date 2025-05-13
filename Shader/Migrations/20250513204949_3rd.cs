using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class _3rd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TransactionPrice",
                table: "MerchantTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransactionPrice",
                table: "ClientTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TransactionPrice",
                table: "CashTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionPrice",
                table: "MerchantTransactionFruits");

            migrationBuilder.DropColumn(
                name: "TransactionPrice",
                table: "ClientTransactionFruits");

            migrationBuilder.DropColumn(
                name: "TransactionPrice",
                table: "CashTransactionFruits");
        }
    }
}
