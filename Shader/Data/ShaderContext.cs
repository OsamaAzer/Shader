using Microsoft.EntityFrameworkCore;
using Shader.Data.Entities;

namespace Shader.Data
{
    public class ShaderContext(DbContextOptions<ShaderContext> options) : DbContext(options)
    {
        public DbSet<Fruit> Fruits { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Owner> Owners { get; set; } = null!;
        public DbSet<CageDetails> CageDetails { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<ClientTransaction> ClientTransactions { get; set; } = null!;
        public DbSet<SupplierTransaction> SupplierTransactions { get; set; } = null!;
        public DbSet<Expense> Expenses { get; set; } = null!;
    }
}
