using System;
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
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMortgageAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DailySalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyEmployees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseTotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseTotalMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseTotalMortgageAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellTotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellTotalMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellTotalMortgageAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BorrowedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyEmployees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientPayments_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyEmpAbsences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyEmpAbsences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyEmpAbsences_DailyEmployees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "DailyEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyEmpSalaryRecordings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DailySalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyEmpSalaryRecordings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyEmpSalaryRecordings_DailyEmployees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "DailyEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MerchantPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    MortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantPayments_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MerchantTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCageMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantTransactions_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmountOfBills = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsMerchant = table.Column<bool>(type: "bit", nullable: false),
                    MerchantId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLoans_MonthlyEmployees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "MonthlyEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyEmpAbsences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyEmpAbsences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyEmpAbsences_MonthlyEmployees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "MonthlyEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyEmpSalaryRecordings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BorrowedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeductionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyEmpSalaryRecordings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyEmpSalaryRecordings_MonthlyEmployees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "MonthlyEmployees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierBills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommissionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MyCommisionValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValueDueToSupplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierBills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierBills_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fruits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FruitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    MerchantAsSupplierPurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MashalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NylonValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceOfKilogramInBill = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsCageHasMortgage = table.Column<bool>(type: "bit", nullable: false),
                    IsBilled = table.Column<bool>(type: "bit", nullable: false),
                    CageMortgageValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCages = table.Column<int>(type: "int", nullable: false),
                    SoldCages = table.Column<int>(type: "int", nullable: false),
                    RemainingCages = table.Column<int>(type: "int", nullable: false),
                    NumberOfKilogramsSold = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceOfKilogramsSold = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupplierBillId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fruits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fruits_SupplierBills_SupplierBillId",
                        column: x => x.SupplierBillId,
                        principalTable: "SupplierBills",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fruits_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCageMortgageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                name: "MerchantTransactionFruits",
                columns: table => new
                {
                    MerchantTransactionId = table.Column<int>(type: "int", nullable: false),
                    FruitId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCages = table.Column<int>(type: "int", nullable: false),
                    WeightInKilograms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceOfKiloGram = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantTransactionFruits", x => new { x.MerchantTransactionId, x.FruitId });
                    table.ForeignKey(
                        name: "FK_MerchantTransactionFruits_Fruits_FruitId",
                        column: x => x.FruitId,
                        principalTable: "Fruits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MerchantTransactionFruits_MerchantTransactions_MerchantTransactionId",
                        column: x => x.MerchantTransactionId,
                        principalTable: "MerchantTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashTransactionFruits",
                columns: table => new
                {
                    CashTransactionId = table.Column<int>(type: "int", nullable: false),
                    FruitId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCages = table.Column<int>(type: "int", nullable: false),
                    WeightInKilograms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceOfKiloGram = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashTransactionFruits", x => new { x.CashTransactionId, x.FruitId });
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
                    ClientTransactionId = table.Column<int>(type: "int", nullable: false),
                    FruitId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCages = table.Column<int>(type: "int", nullable: false),
                    WeightInKilograms = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceOfKiloGram = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTransactionFruits", x => new { x.ClientTransactionId, x.FruitId });
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

            migrationBuilder.CreateIndex(
                name: "IX_CashTransactionFruits_FruitId",
                table: "CashTransactionFruits",
                column: "FruitId");

            migrationBuilder.CreateIndex(
                name: "IX_CashTransactions_FruitId",
                table: "CashTransactions",
                column: "FruitId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPayments_ClientId",
                table: "ClientPayments",
                column: "ClientId");

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
                name: "IX_DailyEmpAbsences_EmployeeId",
                table: "DailyEmpAbsences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyEmpSalaryRecordings_EmployeeId",
                table: "DailyEmpSalaryRecordings",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoans_EmployeeId",
                table: "EmployeeLoans",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fruits_SupplierBillId",
                table: "Fruits",
                column: "SupplierBillId");

            migrationBuilder.CreateIndex(
                name: "IX_Fruits_SupplierId",
                table: "Fruits",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantPayments_MerchantId",
                table: "MerchantPayments",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantTransactionFruits_FruitId",
                table: "MerchantTransactionFruits",
                column: "FruitId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantTransactions_MerchantId",
                table: "MerchantTransactions",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyEmpAbsences_EmployeeId",
                table: "MonthlyEmpAbsences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyEmpSalaryRecordings_EmployeeId",
                table: "MonthlyEmpSalaryRecordings",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierBills_SupplierId",
                table: "SupplierBills",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_MerchantId",
                table: "Suppliers",
                column: "MerchantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashTransactionFruits");

            migrationBuilder.DropTable(
                name: "ClientPayments");

            migrationBuilder.DropTable(
                name: "ClientTransactionFruits");

            migrationBuilder.DropTable(
                name: "DailyEmpAbsences");

            migrationBuilder.DropTable(
                name: "DailyEmpSalaryRecordings");

            migrationBuilder.DropTable(
                name: "EmployeeLoans");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "MerchantPayments");

            migrationBuilder.DropTable(
                name: "MerchantTransactionFruits");

            migrationBuilder.DropTable(
                name: "MonthlyEmpAbsences");

            migrationBuilder.DropTable(
                name: "MonthlyEmpSalaryRecordings");

            migrationBuilder.DropTable(
                name: "CashTransactions");

            migrationBuilder.DropTable(
                name: "ClientTransactions");

            migrationBuilder.DropTable(
                name: "DailyEmployees");

            migrationBuilder.DropTable(
                name: "MerchantTransactions");

            migrationBuilder.DropTable(
                name: "MonthlyEmployees");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Fruits");

            migrationBuilder.DropTable(
                name: "SupplierBills");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Merchants");
        }
    }
}
