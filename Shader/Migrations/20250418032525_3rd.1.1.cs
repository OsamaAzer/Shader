using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class _3rd11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfCagesReceived",
                table: "SupplierTransactionFruits");

            migrationBuilder.DropColumn(
                name: "NumberOfCagesSold",
                table: "ClientTransactionFruits");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCagesReceived",
                table: "SupplierTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfCagesReceived",
                table: "SupplierTransactions");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCagesReceived",
                table: "SupplierTransactionFruits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCagesSold",
                table: "ClientTransactionFruits",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
