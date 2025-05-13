using Shader.Enums;

namespace Shader.Data.Entities
{
    public class MerchantTransaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string? Description { get; set; }
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }
        //public TransactionType TransactionType { get; set; }
        public bool IsDeleted { get; set; } = false;
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalCageMortgageAmount { get; set; }
        public ICollection<MerchantTransactionFruit> MerchantTransactionFruits { get; set; } = new List<MerchantTransactionFruit>();
    }
}
