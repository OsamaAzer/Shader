using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "ClientTransactionFruits");

            migrationBuilder.DropColumn(
                name: "TotalAmountSold",
                table: "ClientTransactionFruits");

            migrationBuilder.AlterColumn<int>(
                name: "TotalCages",
                table: "Fruits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalCages",
                table: "Fruits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "ClientTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountSold",
                table: "ClientTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
