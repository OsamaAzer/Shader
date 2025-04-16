namespace Shader.Data.Entities
{
    public class ClientTransaction : Transaction
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public decimal TotalAmount { get; set; } // المبلغ الإجمالي
        public decimal AmountPaid { get; set; } // المبلغ المدفوع
        public decimal RemainingAmount { get; set; } // المبلغ المتبقي
        public decimal? DiscountAmount { get; set; } // المبلغ المخصوم
        public int? NumberOfCagesTook { get; set; } // عدد الأقفاص المأخوذة
        public decimal WeightInKilograms { get; set; }
        public decimal PricePerKilogram { get; set; } // سعر الكيلو
        
    }
}
