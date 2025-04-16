using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Client : BaseEntity
    {
        public AccountStatus Status { get; set; }
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
        ICollection<ClientTransaction> Transactions { get; set; } = new List<ClientTransaction>();
    }
}
