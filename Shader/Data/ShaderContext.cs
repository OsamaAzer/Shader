using Microsoft.EntityFrameworkCore;
using Shader.Data.Entities;

namespace Shader.Data
{
    public class ShaderContext(DbContextOptions<ShaderContext> options) : DbContext(options)
    {
        public DbSet<Fruit> Fruits { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<SupplierBill> SupplierBills { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<ClientTransaction> ClientTransactions { get; set; }
        public DbSet<ClientPayment> ClientPayments { get; set; }
        public DbSet<ClientTransactionFruit> ClientTransactionFruits { get; set; }
        public DbSet<MerchantTransaction> MerchantTransactions { get; set; }
        public DbSet<MerchantPayment> MerchantPayments { get; set; }
        public DbSet<MerchantTransactionFruit> MerchantTransactionFruits { get; set; }
        public DbSet<CashTransaction> CashTransactions { get; set; }
        public DbSet<CashTransactionFruit> CashTransactionFruits { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<MonthlyEmployee> MonthlyEmployees { get; set; }
        public DbSet<DailyEmployee> DailyEmployees { get; set; }
        public DbSet<MonthlyEmployeeAbsence> MonthlyEmpAbsences { get; set; }
        public DbSet<DailyEmployeeAbsence> DailyEmpAbsences { get; set; }
        public DbSet<DailySalaryRecording> DailySalaryRecordings { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientTransactionFruit>()
                .HasKey(ctf => new { ctf.ClientTransactionId, ctf.FruitId });

            modelBuilder.Entity<ClientTransactionFruit>()
                .HasOne(ctf => ctf.ClientTransaction)
                .WithMany(ct => ct.ClientTransactionFruits)
                .HasForeignKey(ctf => ctf.ClientTransactionId);

            modelBuilder.Entity<ClientTransactionFruit>()
                .HasOne(ctf => ctf.Fruit)
                .WithMany()
                .HasForeignKey(ctf => ctf.FruitId);

            modelBuilder.Entity<MerchantTransactionFruit>()
                .HasKey(ctf => new { ctf.MerchantTransactionId, ctf.FruitId });

            modelBuilder.Entity<MerchantTransactionFruit>()
                .HasOne(ctf => ctf.MerchantTransaction)
                .WithMany(ct => ct.MerchantTransactionFruits)
                .HasForeignKey(ctf => ctf.MerchantTransactionId);

            modelBuilder.Entity<MerchantTransactionFruit>()
                .HasOne(ctf => ctf.Fruit)
                .WithMany()
                .HasForeignKey(ctf => ctf.FruitId);

            modelBuilder.Entity<CashTransactionFruit>()
                .HasKey(ctf => new { ctf.CashTransactionId, ctf.FruitId });

            modelBuilder.Entity<CashTransactionFruit>()
                .HasOne(ctf => ctf.CashTransaction)
                .WithMany(ct => ct.CashTransactionFruits)
                .HasForeignKey(ctf => ctf.CashTransactionId);

            modelBuilder.Entity<CashTransactionFruit>()
                .HasOne(ctf => ctf.Fruit)
                .WithMany()
                .HasForeignKey(ctf => ctf.FruitId);
        }
    }
}
