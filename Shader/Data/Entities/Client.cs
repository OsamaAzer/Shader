using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string City { get; set; }
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
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
        public ICollection<ClientTransaction> Transactions { get; set; } = new List<ClientTransaction>();
    }
}
