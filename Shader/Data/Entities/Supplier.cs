using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; }
        public decimal TotalAmountOfBills { get; set; } // المبلغ الإجمالي للفواتير
        public decimal TotalAmountPaid { get; set; } // المبلغ المدفوع
        public decimal TotalRemainingAmount { get; set; } // المبلغ المتبقي
        public bool IsMerchant { get; set; } = false; // هل المورد تاجر
        public int? MerchantId { get; set; }
        public Merchant? Merchant { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<SupplierBill> SupplierBills { get; set; } = new List<SupplierBill>();
        public ICollection<Fruit> Fruits { get; set; } = new List<Fruit>();
    }
}
