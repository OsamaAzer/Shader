using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class ClientTransaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } 
        public string? Description { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public bool IsDeleted { get; set; } = false;
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; } 
        public decimal TotalCageMortgageAmount { get; set; }
        public ICollection<ClientTransactionFruit> ClientTransactionFruits { get; set; } = new List<ClientTransactionFruit>();
    }
}
