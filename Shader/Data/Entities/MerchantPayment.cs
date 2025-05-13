using Shader.Enums;
using System.Transactions;

namespace Shader.Data.Entities
{
    public class MerchantPayment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal MortgageAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }
        public bool IsDeleted { get; set; }
    }
}
