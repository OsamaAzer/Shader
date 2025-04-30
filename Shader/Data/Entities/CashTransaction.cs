using Shader.Enums;

namespace Shader.Data.Entities
{
    public class CashTransaction
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; } 
        public ICollection<CashTransactionFruit> CashTransactionFruits { get; set; } = new List<CashTransactionFruit>();
    }

}
