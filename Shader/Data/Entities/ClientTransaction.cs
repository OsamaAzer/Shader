using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class ClientTransaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        [MaxLength(100)]
        public string? Description { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [Required]
        public decimal TotalAmount { get; set; } // المبلغ الإجمالي
        [Required]
        public decimal AmountPaid { get; set; } // المبلغ المدفوع
        [Required]
        public decimal RemainingAmount { get; set; } // المبلغ المتبقي
        public decimal? DiscountAmount { get; set; } // المبلغ المخصوم
        [Required]
        public int NumberOfCagesTook { get; set; } // عدد الأقفاص المأخوذة
        public decimal? TotalCageMortgageAmount { get; set; } // قيمة الرهن
        public decimal? TotalCageMortgageAmountPaid { get; set; } // قيمة الرهن المدفوع
        public decimal? RemainingMortgageAmount { get; set; } // قيمة الرهن المتبقي
        [Required]
        public decimal WeightInKilograms { get; set; }
        [Required]
        public decimal PricePerKilogram { get; set; } // سعر الكيلو
        public ICollection<ClientTransactionFruit> ClientTransactionFruits { get; set; } = new List<ClientTransactionFruit>();
    }
}
