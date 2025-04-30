using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Fruit
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string FruitName { get; set; }
        public FruitStatus Status { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public bool IsCageHasMortgage { get; set; } = false;
        public decimal CageMortgageValue { get; set; } // قيمة رهن القفص
        public int NumberOfMortgagePaidCages { get; set; } // عدد القفص المدفوع الرهن
        public int TotalCages { get; set; }
        public int SoldCages { get; set; }
        public int RemainingCages { get; set; }
        public decimal NumberOfKilogramsSold { get; set; }
        public decimal PriceOfKilogramsSold { get; set; }
        public ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>(); // المعاملات المرتبطة بالفواكه
        public ICollection<CashTransaction> CashTransactions { get; set; } = new List<CashTransaction>(); // المعاملات المرتبطة بالفواكه

    }
}
