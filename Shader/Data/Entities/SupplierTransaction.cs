using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class SupplierTransaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string? Description { get; set; }
        [Required]
        public int NumberOfCagesReceived { get; set; } // عدد الأقفاص المستلمة
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<SupplierTransactionFruit> SupplierTransactionFruits { get; set; } = new List<SupplierTransactionFruit>();
    }
}
