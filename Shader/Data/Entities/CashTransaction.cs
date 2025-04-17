using Shader.Enums;

namespace Shader.Data.Entities
{
    public class CashTransaction
    {
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; } // المبلغ
        public decimal PriceOfKiloGram { get; set; }
        public decimal WeightInKilograms { get; set; } // الوزن بالكيلو
        public ICollection<CashTransactionFruit> CashTransactionFruits { get; set; } = new List<CashTransactionFruit>();
    }

}
