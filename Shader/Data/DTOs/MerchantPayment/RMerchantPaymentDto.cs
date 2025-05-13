using Shader.Enums;

namespace Shader.Data.DTOs.MerchantPayment
{
    public class RMerchantPaymentDto
    {
        public string MerchantName { get; set; } 
        public TransactionType TransactionType { get; set; }
        public DateTime Date { get; set; }
        public decimal MortgageAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
