using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string City { get; set; } 
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; }
        public ClientStatus Status { get; set; }
        public bool IsDeleted { get; set; } = false;
        public decimal Price { get; set; } // المبلغ الكلي
        public decimal TotalDiscountAmount { get; set; } // المبلغ المخصوم
        public decimal TotalAmount => Price - TotalDiscountAmount; // المبلغ الإجمالي بعد الخصم
        public decimal AmountPaid { get; set; }
        public decimal TotalRemainingAmount => TotalAmount - AmountPaid; // المبلغ المتبقي
        public decimal TotalMortgageAmount { get; set; } // قيمة الرهن
        public decimal TotalMortgageAmountPaid { get; set; } // قيمة الرهن المدفوع
        public decimal TotalRemainingMortgageAmount => TotalMortgageAmount - TotalMortgageAmountPaid; // المبلغ المتبقي من الرهن
        public ICollection<ClientPayment> Payments { get; set; } = new List<ClientPayment>();
        public ICollection<ClientTransaction> Transactions { get; set; } = new List<ClientTransaction>();
    }
}
