using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class _2nd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellingTotalRemainingMortgageAmount",
                table: "Merchants",
                newName: "SellTotalRemainingMortgageAmount");

            migrationBuilder.RenameColumn(
                name: "SellingTotalRemainingAmount",
                table: "Merchants",
                newName: "SellTotalRemainingAmount");

            migrationBuilder.RenameColumn(
                name: "SellingTotalMortgageAmountPaid",
                table: "Merchants",
                newName: "SellTotalMortgageAmountPaid");

            migrationBuilder.RenameColumn(
                name: "SellingTotalMortgageAmount",
                table: "Merchants",
                newName: "SellTotalMortgageAmount");

            migrationBuilder.RenameColumn(
                name: "SellingTotalDiscountAmount",
                table: "Merchants",
                newName: "SellTotalDiscountAmount");

            migrationBuilder.RenameColumn(
                name: "SellingTotalAmount",
                table: "Merchants",
                newName: "SellTotalAmount");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "Merchants",
                newName: "SellPrice");

            migrationBuilder.RenameColumn(
                name: "SellingAmountPaid",
                table: "Merchants",
                newName: "SellAmountPaid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellTotalRemainingMortgageAmount",
                table: "Merchants",
                newName: "SellingTotalRemainingMortgageAmount");

            migrationBuilder.RenameColumn(
                name: "SellTotalRemainingAmount",
                table: "Merchants",
                newName: "SellingTotalRemainingAmount");

            migrationBuilder.RenameColumn(
                name: "SellTotalMortgageAmountPaid",
                table: "Merchants",
                newName: "SellingTotalMortgageAmountPaid");

            migrationBuilder.RenameColumn(
                name: "SellTotalMortgageAmount",
                table: "Merchants",
                newName: "SellingTotalMortgageAmount");

            migrationBuilder.RenameColumn(
                name: "SellTotalDiscountAmount",
                table: "Merchants",
                newName: "SellingTotalDiscountAmount");

            migrationBuilder.RenameColumn(
                name: "SellTotalAmount",
                table: "Merchants",
                newName: "SellingTotalAmount");

            migrationBuilder.RenameColumn(
                name: "SellPrice",
                table: "Merchants",
                newName: "SellingPrice");

            migrationBuilder.RenameColumn(
                name: "SellAmountPaid",
                table: "Merchants",
                newName: "SellingAmountPaid");
        }
    }
}
