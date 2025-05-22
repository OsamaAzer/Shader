namespace Shader.Data.Entities
{
    public class SupplierBill
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } 
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Price { get; set; } // السعر
        public decimal TotalAmount { get; set; } // المبلغ الإجمالي
        public decimal CommissionRate { get; set; } // نسبة العمولة
        public decimal MyCommisionValue { get; set; } // قيمة العمولة الخاصة بي
        public decimal ValueDueToSupplier { get; set; } // المبلغ المستحق للمورد
        public ICollection<Fruit> Fruits { get; set; } = new List<Fruit>();
    }
}
