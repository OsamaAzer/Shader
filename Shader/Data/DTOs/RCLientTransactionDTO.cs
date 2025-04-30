using Shader.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs
{
    public class RCLientTransactionDTO
    {
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? Description { get; set; }
        public Client Client { get; set; }
        public decimal TotalAmount { get; set; } // المبلغ الإجمالي
        public decimal AmountPaid { get; set; } // المبلغ المدفوع
        public decimal RemainingAmount { get; set; } // المبلغ المتبقي
        public decimal DiscountAmount { get; set; } // المبلغ المخصوم
        public decimal TotalCageMortgageAmount { get; set; } // قيمة الرهن
        public decimal TotalCageMortgageAmountPaid { get; set; } // قيمة الرهن المدفوع
        public decimal RemainingMortgageAmount { get; set; } // قيمة الرهن المتبقي
        public List<ClientTransactionFruit> ClientTransactionFruits { get; set; }
    }
}
