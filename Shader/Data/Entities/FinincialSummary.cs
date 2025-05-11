namespace Shader.Data.Entities
{
    public class FinincialSummary
    {
        public decimal Price { get; set; } // المبلغ الكلي
        public decimal PriceReceived { get; set; } // المبلغ المدفوع
        public decimal TotalRemainingAmount { get; set; } // المبلغ المتبقي
        public decimal AmountOfRateTookFromSuppliers { get; set; } // المبلغ الذي تم أخذه من الموردين
        public decimal TotalDiscountAmount { get; set; } // المبلغ المخصوم
    }
}
