using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shader.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CageDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CageMortgageValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumberOfMortgagePaidCages = table.Column<int>(type: "int", nullable: true),
                    TotalCages = table.Column<int>(type: "int", nullable: false),
                    SoldCages = table.Column<int>(type: "int", nullable: false),
                    RemainingCages = table.Column<int>(type: "int", nullable: false),
                    ReturnedCages = table.Column<int>(type: "int", nullable: false),
                    UnReturnedCages = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CageDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMortgageAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRemainingMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNumberOfCagesTook = table.Column<int>(type: "int", nullable: false),
                    TotalNumberOfCagesReturned = table.Column<int>(type: "int", nullable: false),
                    TotalNumberOfUnReturnedCages = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountOfRateTookFromSuppliers = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fruits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FruitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmountSold = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmountReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmountRemain = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CageDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fruits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fruits_CageDetails_CageDetailsId",
                        column: x => x.CageDetailsId,
                        principalTable: "CageDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumberOfCagesTook = table.Column<int>(type: "int", nullable: true),
                    WeightInKilograms = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PricePerKilogram = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OwnerRate = table.Column<double>(type: "float", nullable: true),
                    OwnerRateAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fruits_CageDetailsId",
                table: "Fruits",
                column: "CageDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ClientId",
                table: "Transactions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SupplierId",
                table: "Transactions",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Fruits");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "CageDetails");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
