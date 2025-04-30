using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class _313 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierTransactionFruits");

            migrationBuilder.DropTable(
                name: "SupplierTransactions");

            migrationBuilder.DropColumn(
                name: "ReturnedCages",
                table: "Fruits");

            migrationBuilder.DropColumn(
                name: "TotalAmountReceived",
                table: "Fruits");

            migrationBuilder.DropColumn(
                name: "TotalAmountRemain",
                table: "Fruits");

            migrationBuilder.DropColumn(
                name: "TotalAmountSold",
                table: "Fruits");

            migrationBuilder.DropColumn(
                name: "UnReturnedCages",
                table: "Fruits");

            migrationBuilder.AlterColumn<int>(
                name: "TotalCages",
                table: "Fruits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SoldCages",
                table: "Fruits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RemainingCages",
                table: "Fruits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfMortgagePaidCages",
                table: "Fruits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCageHasMortgage",
                table: "Fruits",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CageMortgageValue",
                table: "Fruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Fruits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Fruits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fruits_SupplierId",
                table: "Fruits",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fruits_Suppliers_SupplierId",
                table: "Fruits",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fruits_Suppliers_SupplierId",
                table: "Fruits");

            migrationBuilder.DropIndex(
                name: "IX_Fruits_SupplierId",
                table: "Fruits");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Fruits");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Fruits");

            migrationBuilder.AlterColumn<int>(
                name: "TotalCages",
                table: "Fruits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SoldCages",
                table: "Fruits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RemainingCages",
                table: "Fruits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfMortgagePaidCages",
                table: "Fruits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCageHasMortgage",
                table: "Fruits",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<decimal>(
                name: "CageMortgageValue",
                table: "Fruits",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "ReturnedCages",
                table: "Fruits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountReceived",
                table: "Fruits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountRemain",
                table: "Fruits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmountSold",
                table: "Fruits",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnReturnedCages",
                table: "Fruits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SupplierTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FruitId = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_Fruits_FruitId",
                        column: x => x.FruitId,
                        principalTable: "Fruits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplierTransactions_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierTransactionFruits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FruitId = table.Column<int>(type: "int", nullable: false),
                    SupplierTransactionId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCagesReceived = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierTransactionFruits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierTransactionFruits_Fruits_FruitId",
                        column: x => x.FruitId,
                        principalTable: "Fruits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierTransactionFruits_SupplierTransactions_SupplierTransactionId",
                        column: x => x.SupplierTransactionId,
                        principalTable: "SupplierTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactionFruits_FruitId",
                table: "SupplierTransactionFruits",
                column: "FruitId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactionFruits_SupplierTransactionId",
                table: "SupplierTransactionFruits",
                column: "SupplierTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_FruitId",
                table: "SupplierTransactions",
                column: "FruitId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierTransactions_SupplierId",
                table: "SupplierTransactions",
                column: "SupplierId");
        }
    }
}
