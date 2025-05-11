using Shader.Data.Dtos.Fruit;
using Shader.Data.Entities;

namespace Shader.Data.DTOs.SupplierBill
{
    public class RSupplierBillDto
    {
        public string SupplierName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; } // المبلغ الإجمالي
        public decimal CommissionRate { get; set; }
        public decimal MyCommisionValue { get; set; } // قيمة العمولة الخاصة بي
        public decimal ValueDueToSupplier { get; set; } // المبلغ المستحق للمورد
        public decimal MshalValue { get; set; } // قيمة المشال
        public decimal NylonValue { get; set; } // قيمة النايلون
        public List<RAllFruitsDto> Fruits { get; set; } 

    }
}
