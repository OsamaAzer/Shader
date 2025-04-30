using Shader.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs
{
    public class WClientTransactionDTO
    {
        public string? Description { get; set; }
        [Required]
        public int ClientId { get; set; }
        public decimal AmountPaid { get; set; } // المبلغ المدفوع
        public decimal DiscountAmount { get; set; } // المبلغ المخصوم
        public decimal TotalCageMortgageAmountPaid { get; set; } // قيمة الرهن المدفوع
        public List<ClientTransactionFruitDTO> ClientTransactionFruits { get; set; }
    }
}
