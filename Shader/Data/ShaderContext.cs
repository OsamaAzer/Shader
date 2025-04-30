using Microsoft.EntityFrameworkCore;
using Shader.Data.Entities;

namespace Shader.Data
{
    public class ShaderContext(DbContextOptions<ShaderContext> options) : DbContext(options)
    {
        public DbSet<Fruit> Fruits { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientTransaction> ClientTransactions { get; set; }
        public DbSet<ClientTransactionFruit> ClientTransactionFruits { get; set; }
        public DbSet<CashTransaction> CashTransactions { get; set; }
        public DbSet<CashTransactionFruit> CashTransactionFruits { get; set; }
        public DbSet<Expense> Expenses { get; set; }

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
            
            modelBuilder.Entity<CashTransactionFruit>()
                .HasOne(ctf => ctf.CashTransaction)
                .WithMany(ct => ct.CashTransactionFruits)
                .HasForeignKey(ctf => ctf.CashTransactionId);

            modelBuilder.Entity<CashTransactionFruit>()
                .HasOne(ctf => ctf.Fruit)
                .WithMany()
                .HasForeignKey(ctf => ctf.FruitId);
            
            //modelBuilder.Entity<Expense>()
            //    .Property(e => e.Amount)
            //    .HasColumnType("decimal(18, 2)");
        }
    }
}
