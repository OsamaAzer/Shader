using Shader.Enums;

namespace Shader.Data.Entities
{
    public class Fruit
    {
        public int Id { get; set; }
        public string FruitName { get; set; }
        public decimal TotalAmountSold { get; set; }  // المبلغ الكلي المباع
        public decimal TotalAmountReceived { get; set; }  // المبلغ المدفوع
        public decimal TotalAmountRemain { get; set; }  // المبلغ المتبقي
        public int CageDetailsId { get; set; }
        public CageDetails CageDetails { get; set; } // تفاصيل القفص المرتبطة بالفواكه
        ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); // المعاملات المرتبطة بالفواكه
        ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>(); // الموردين المرتبطين بالفواكه
    }
}
