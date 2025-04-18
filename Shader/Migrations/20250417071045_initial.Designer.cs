﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shader.Data;

#nullable disable

namespace Shader.Migrations
{
    [DbContext(typeof(ShaderContext))]
    [Migration("20250417071045_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CashTransactionFruit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CashTransactionId")
                        .HasColumnType("int");

                    b.Property<int>("FruitId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfCages")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CashTransactionId");

                    b.HasIndex("FruitId");

                    b.ToTable("CashTransactionFruits");
                });

            modelBuilder.Entity("ClientTransactionFruit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientTransactionId")
                        .HasColumnType("int");

                    b.Property<int>("FruitId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfCagesSold")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientTransactionId");

                    b.HasIndex("FruitId");

                    b.ToTable("ClientTransactionFruits");
                });

            modelBuilder.Entity("Shader.Data.Entities.CashTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FruitId")
                        .HasColumnType("int");

                    b.Property<decimal>("PriceOfKiloGram")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("WeightInKilograms")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("FruitId");

                    b.ToTable("CashTransactions");
                });

            modelBuilder.Entity("Shader.Data.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateOnly?>("DateOfLastTransaction")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalAmountPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalDiscountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalMortgageAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalMortgageAmountPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TotalNumberOfCagesReturned")
                        .HasColumnType("int");

                    b.Property<int>("TotalNumberOfCagesTook")
                        .HasColumnType("int");

                    b.Property<int>("TotalNumberOfUnReturnedCages")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalRemainingAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalRemainingMortgageAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Shader.Data.Entities.ClientTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AmountPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal?>("DiscountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("FruitId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfCagesTook")
                        .HasColumnType("int");

                    b.Property<decimal>("PricePerKilogram")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RemainingAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("RemainingMortgageAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalCageMortgageAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalCageMortgageAmountPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("WeightInKilograms")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("FruitId");

                    b.ToTable("ClientTransactions");
                });

            modelBuilder.Entity("Shader.Data.Entities.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Shader.Data.Entities.Fruit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal?>("CageMortgageValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("FruitName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool?>("IsCageHasMortgage")
                        .HasColumnType("bit");

                    b.Property<int?>("NumberOfMortgagePaidCages")
                        .HasColumnType("int");

                    b.Property<int?>("RemainingCages")
                        .HasColumnType("int");

                    b.Property<int?>("ReturnedCages")
                        .HasColumnType("int");

                    b.Property<int?>("SoldCages")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalAmountReceived")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalAmountRemain")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalAmountSold")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("TotalCages")
                        .HasColumnType("int");

                    b.Property<int?>("UnReturnedCages")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Fruits");
                });

            modelBuilder.Entity("Shader.Data.Entities.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateOnly?>("DateOfLastTransaction")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Shader.Data.Entities.SupplierTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FruitId")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FruitId");

                    b.HasIndex("SupplierId");

                    b.ToTable("SupplierTransactions");
                });

            modelBuilder.Entity("SupplierTransactionFruit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FruitId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfCagesReceived")
                        .HasColumnType("int");

                    b.Property<int>("SupplierTransactionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FruitId");

                    b.HasIndex("SupplierTransactionId");

                    b.ToTable("SupplierTransactionFruits");
                });

            modelBuilder.Entity("CashTransactionFruit", b =>
                {
                    b.HasOne("Shader.Data.Entities.CashTransaction", "CashTransaction")
                        .WithMany("CashTransactionFruits")
                        .HasForeignKey("CashTransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shader.Data.Entities.Fruit", "Fruit")
                        .WithMany()
                        .HasForeignKey("FruitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CashTransaction");

                    b.Navigation("Fruit");
                });

            modelBuilder.Entity("ClientTransactionFruit", b =>
                {
                    b.HasOne("Shader.Data.Entities.ClientTransaction", "ClientTransaction")
                        .WithMany("ClientTransactionFruits")
                        .HasForeignKey("ClientTransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shader.Data.Entities.Fruit", "Fruit")
                        .WithMany()
                        .HasForeignKey("FruitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClientTransaction");

                    b.Navigation("Fruit");
                });

            modelBuilder.Entity("Shader.Data.Entities.CashTransaction", b =>
                {
                    b.HasOne("Shader.Data.Entities.Fruit", null)
                        .WithMany("CashTransactions")
                        .HasForeignKey("FruitId");
                });

            modelBuilder.Entity("Shader.Data.Entities.ClientTransaction", b =>
                {
                    b.HasOne("Shader.Data.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shader.Data.Entities.Fruit", null)
                        .WithMany("ClientTransactions")
                        .HasForeignKey("FruitId");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Shader.Data.Entities.SupplierTransaction", b =>
                {
                    b.HasOne("Shader.Data.Entities.Fruit", null)
                        .WithMany("SupplierTransactions")
                        .HasForeignKey("FruitId");

                    b.HasOne("Shader.Data.Entities.Supplier", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("SupplierTransactionFruit", b =>
                {
                    b.HasOne("Shader.Data.Entities.Fruit", "Fruit")
                        .WithMany()
                        .HasForeignKey("FruitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shader.Data.Entities.SupplierTransaction", "SupplierTransaction")
                        .WithMany("SupplierTransactionFruits")
                        .HasForeignKey("SupplierTransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fruit");

                    b.Navigation("SupplierTransaction");
                });

            modelBuilder.Entity("Shader.Data.Entities.CashTransaction", b =>
                {
                    b.Navigation("CashTransactionFruits");
                });

            modelBuilder.Entity("Shader.Data.Entities.ClientTransaction", b =>
                {
                    b.Navigation("ClientTransactionFruits");
                });

            modelBuilder.Entity("Shader.Data.Entities.Fruit", b =>
                {
                    b.Navigation("CashTransactions");

                    b.Navigation("ClientTransactions");

                    b.Navigation("SupplierTransactions");
                });

            modelBuilder.Entity("Shader.Data.Entities.SupplierTransaction", b =>
                {
                    b.Navigation("SupplierTransactionFruits");
                });
#pragma warning restore 612, 618
        }
    }
}
