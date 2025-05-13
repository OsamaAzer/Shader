using Shader.Enums;

namespace Shader.Data.Dtos.Client
{
    public class RClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public Status Status { get; set; }
        public decimal Price { get; set; } // المبلغ الكلي
        public decimal TotalAmount { get; set; } // المبلغ بعد الخصم
        public decimal AmountPaid { get; set; }
        public decimal TotalRemainingAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; } // المبلغ المخصوم
        public decimal TotalMortgageAmount { get; set; } // قيمة الرهن
        public decimal TotalMortgageAmountPaid { get; set; } // قيمة الرهن المدفوع
        public decimal TotalRemainingMortgageAmount { get; set; } // قيمة الرهن المتبقي
    }
}
