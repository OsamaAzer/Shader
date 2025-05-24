using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Fruit
    {
        public int Id { get; set; }
        public string FruitName { get; set; } = null!;
        public FruitStatus Status { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;
        public decimal MerchantAsSupplierPurchasePrice { get; set; } // سعر الشراء من شادر مسجل كمورد
        public decimal MashalValue { get; set; }
        public decimal NylonValue { get; set; }
        public decimal PriceOfKilogramInBill { get; set; } // السعر فى الفاتوره
        public bool IsDeleted { get; set; } = false;
        public bool IsCageHasMortgage { get; set; } = false;
        public bool IsBilled { get; set; } = false; // هل تم إصدار فاتورة للفواكه
        public decimal CageMortgageValue { get; set; } // قيمة رهن القفص
        public int TotalCages { get; set; }
        public int SoldCages { get; set; }
        public int RemainingCages { get; set; }
        public decimal NumberOfKilogramsSold { get; set; }
        public decimal PriceOfKilogramsSold { get; set; }
        public ICollection<ClientTransaction> ClientTransactions { get; set; } = new List<ClientTransaction>(); // المعاملات المرتبطة بالفواكه
        public ICollection<CashTransaction> CashTransactions { get; set; } = new List<CashTransaction>(); // المعاملات المرتبطة بالفواكه

        // todo : flag to check if the fruit has a bill
    }
}
