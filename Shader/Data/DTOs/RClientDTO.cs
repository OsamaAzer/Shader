using Shader.Enums;

namespace Shader.Data.DTOs
{
    public class RClientDTO
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly? DateOfLastTransaction { get; set; }
        public Status Status { get; set; }
        public decimal TotalAmount { get; set; } // المبلغ الكلي
        public decimal TotalAmountPaid { get; set; }
        public decimal TotalRemainingAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; } // المبلغ المخصوم
        public decimal TotalMortgageAmount { get; set; } // قيمة الرهن
        public decimal TotalMortgageAmountPaid { get; set; } // قيمة الرهن المدفوع
        public decimal TotalRemainingMortgageAmount { get; set; } // قيمة الرهن المتبقي
        public int TotalNumberOfCagesTook { get; set; }
        public int TotalNumberOfCagesReturned { get; set; }
        public int TotalNumberOfUnReturnedCages { get; set; }
    }
}
