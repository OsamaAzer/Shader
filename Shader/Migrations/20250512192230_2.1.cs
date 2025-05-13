using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class _21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "MerchantTransactions");

            migrationBuilder.AddColumn<bool>(
                name: "IsMerchant",
                table: "Suppliers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MerchantId",
                table: "Suppliers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MerchantPurchasePrice",
                table: "Fruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_MerchantId",
                table: "Suppliers",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Merchants_MerchantId",
                table: "Suppliers",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Merchants_MerchantId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_MerchantId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IsMerchant",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "MerchantPurchasePrice",
                table: "Fruits");

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "MerchantTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
