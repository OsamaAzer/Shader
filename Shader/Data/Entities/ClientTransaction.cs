using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class ClientTransaction
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; } 
        public TimeOnly Time { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public decimal TotalAmount { get; set; } 
        [Required]
        public decimal AmountPaid { get; set; } 
        public decimal RemainingAmount { get; set; } 
        public decimal DiscountAmount { get; set; } 
        public decimal TotalCageMortgageAmount { get; set; } 
        public decimal TotalCageMortgageAmountPaid { get; set; } 
        public decimal RemainingMortgageAmount { get; set; } 
        public ICollection<ClientTransactionFruit> ClientTransactionFruits { get; set; } = new List<ClientTransactionFruit>();
    }
}
