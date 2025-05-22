using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class CashTransaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public string? Description { get; set; }
        public decimal Price { get; set; } 
        public ICollection<CashTransactionFruit> CashTransactionFruits { get; set; } = new List<CashTransactionFruit>();
    }

}
