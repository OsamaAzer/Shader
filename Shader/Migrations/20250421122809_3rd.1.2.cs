using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class _3rd12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfCagesReceived",
                table: "SupplierTransactions");

            migrationBuilder.DropColumn(
                name: "NumberOfCagesTook",
                table: "ClientTransactions");

            migrationBuilder.DropColumn(
                name: "PricePerKilogram",
                table: "ClientTransactions");

            migrationBuilder.DropColumn(
                name: "WeightInKilograms",
                table: "ClientTransactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "CashTransactions");

            migrationBuilder.DropColumn(
                name: "PriceOfKiloGram",
                table: "CashTransactions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CashTransactions");

            migrationBuilder.RenameColumn(
                name: "WeightInKilograms",
                table: "CashTransactions",
                newName: "TotalAmount");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "SupplierTransactions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "SupplierTransactions",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCagesReceived",
                table: "SupplierTransactionFruits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCageMortgageAmount",
                table: "ClientTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "ClientTransactions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "ClientTransactions",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "ClientTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NumberOfCages",
                table: "ClientTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceOfKiloGram",
                table: "ClientTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightInKilograms",
                table: "ClientTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "CashTransactions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "CashTransactions",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

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

            migrationBuilder.AddColumn<decimal>(
                name: "PriceOfKiloGram",
                table: "CashTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightInKilograms",
                table: "CashTransactionFruits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "SupplierTransactions");

            migrationBuilder.DropColumn(
                name: "NumberOfCagesReceived",
                table: "SupplierTransactionFruits");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "ClientTransactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ClientTransactionFruits");

            migrationBuilder.DropColumn(
                name: "NumberOfCages",
                table: "ClientTransactionFruits");

            migrationBuilder.DropColumn(
                name: "PriceOfKiloGram",
                table: "ClientTransactionFruits");

            migrationBuilder.DropColumn(
                name: "WeightInKilograms",
                table: "ClientTransactionFruits");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "CashTransactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "CashTransactionFruits");

            migrationBuilder.DropColumn(
                name: "PriceOfKiloGram",
                table: "CashTransactionFruits");

            migrationBuilder.DropColumn(
                name: "WeightInKilograms",
                table: "CashTransactionFruits");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "CashTransactions",
                newName: "WeightInKilograms");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "SupplierTransactions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCagesReceived",
                table: "SupplierTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCageMortgageAmount",
                table: "ClientTransactions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "ClientTransactions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfCagesTook",
                table: "ClientTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerKilogram",
                table: "ClientTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WeightInKilograms",
                table: "ClientTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "CashTransactions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "CashTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceOfKiloGram",
                table: "CashTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "CashTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfCages",
                table: "CashTransactionFruits",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
