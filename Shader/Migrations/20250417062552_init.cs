using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfLastTransaction = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMortgageAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRemainingMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNumberOfCagesTook = table.Column<int>(type: "int", nullable: false),
                    TotalNumberOfCagesReturned = table.Column<int>(type: "int", nullable: false),
                    TotalNumberOfUnReturnedCages = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fruits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FruitName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalAmountSold = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmountReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmountRemain = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsCageHasMortgage = table.Column<bool>(type: "bit", nullable: true),
                    CageMortgageValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumberOfMortgagePaidCages = table.Column<int>(type: "int", nullable: true),
                    TotalCages = table.Column<int>(type: "int", nullable: false),
                    SoldCages = table.Column<int>(type: "int", nullable: true),
                    RemainingCages = table.Column<int>(type: "int", nullable: true),
                    ReturnedCages = table.Column<int>(type: "int", nullable: true),
                    UnReturnedCages = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fruits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfLastTransaction = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceOfKiloGram = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeightInKilograms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FruitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashTransactions_Fruits_FruitId",
                        column: x => x.FruitId,
                        principalTable: "Fruits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumberOfCagesTook = table.Column<int>(type: "int", nullable: false),
                    TotalCageMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCageMortgageAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WeightInKilograms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerKilogram = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FruitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientTransactions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientTransactions_Fruits_FruitId",
                        column: x => x.FruitId,
                        principalTable: "Fruits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupplierTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    FruitId = table.Column<int>(type: "int", nullable: true)
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
                name: "CashTransactionFruits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashTransactionId = table.Column<int>(type: "int", nullable: false),
                    FruitId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCages = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTransactionFruits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashTransactionFruits_CashTransactions_CashTransactionId",
                        column: x => x.CashTransactionId,
                        principalTable: "CashTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CashTransactionFruits_Fruits_FruitId",
                        column: x => x.FruitId,
                        principalTable: "Fruits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientTransactionFruits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientTransactionId = table.Column<int>(type: "int", nullable: false),
                    FruitId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCagesSold = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmountSold = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTransactionFruits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientTransactionFruits_ClientTransactions_ClientTransactionId",
                        column: x => x.ClientTransactionId,
                        principalTable: "ClientTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientTransactionFruits_Fruits_FruitId",
                        column: x => x.FruitId,
                        principalTable: "Fruits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierTransactionFruits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierTransactionId = table.Column<int>(type: "int", nullable: false),
                    FruitId = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_CashTransactionFruits_CashTransactionId",
                table: "CashTransactionFruits",
                column: "CashTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CashTransactionFruits_FruitId",
                table: "CashTransactionFruits",
                column: "FruitId");

            migrationBuilder.CreateIndex(
                name: "IX_CashTransactions_FruitId",
                table: "CashTransactions",
                column: "FruitId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTransactionFruits_ClientTransactionId",
                table: "ClientTransactionFruits",
                column: "ClientTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTransactionFruits_FruitId",
                table: "ClientTransactionFruits",
                column: "FruitId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTransactions_ClientId",
                table: "ClientTransactions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTransactions_FruitId",
                table: "ClientTransactions",
                column: "FruitId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashTransactionFruits");

            migrationBuilder.DropTable(
                name: "ClientTransactionFruits");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "SupplierTransactionFruits");

            migrationBuilder.DropTable(
                name: "CashTransactions");

            migrationBuilder.DropTable(
                name: "ClientTransactions");

            migrationBuilder.DropTable(
                name: "SupplierTransactions");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Fruits");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
