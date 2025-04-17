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
        public decimal? TotalAmountSold { get; set; }  // المبلغ الكلي المباع
        public decimal? TotalAmountReceived { get; set; }  // المبلغ المدفوع
        public decimal? TotalAmountRemain { get; set; }  // المبلغ المتبقي
        public bool? IsCageHasMortgage { get; set; } = false;
        public decimal? CageMortgageValue { get; set; } // قيمة رهن القفص
        public int? NumberOfMortgagePaidCages { get; set; } // عدد القفص المدفوع الرهن
        public int? TotalCages { get; set; }
        public int? SoldCages { get; set; }
        public int? RemainingCages { get; set; }
        public int? ReturnedCages { get; set; }
        public int? UnReturnedCages { get; set; }
        public ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>(); // المعاملات المرتبطة بالفواكه
        public ICollection<SupplierTransaction> SupplierTransactions { get; set; } = new List<SupplierTransaction>(); // المعاملات المرتبطة بالفواكه
        public ICollection<CashTransaction> CashTransactions { get; set; } = new List<CashTransaction>(); // المعاملات المرتبطة بالفواكه

    }
}
