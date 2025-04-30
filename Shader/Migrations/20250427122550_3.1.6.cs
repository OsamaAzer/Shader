using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class _316 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientTransactionFruits",
                table: "ClientTransactionFruits");

            migrationBuilder.DropIndex(
                name: "IX_ClientTransactionFruits_ClientTransactionId",
                table: "ClientTransactionFruits");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ClientTransactionFruits");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ClientTransactionFruits");

            migrationBuilder.AlterColumn<decimal>(
                name: "RemainingMortgageAmount",
                table: "ClientTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientTransactionFruits",
                table: "ClientTransactionFruits",
                columns: new[] { "ClientTransactionId", "FruitId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientTransactionFruits",
                table: "ClientTransactionFruits");

            migrationBuilder.AlterColumn<decimal>(
                name: "RemainingMortgageAmount",
                table: "ClientTransactions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ClientTransactionFruits",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "ClientTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientTransactionFruits",
                table: "ClientTransactionFruits",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTransactionFruits_ClientTransactionId",
                table: "ClientTransactionFruits",
                column: "ClientTransactionId");
        }
    }
}
