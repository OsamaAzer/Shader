using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Merchant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; } = false;
        public decimal PurchasePrice { get; set; } // المبلغ الكلي
        public decimal PurchaseTotalAmount { get; set; } // المبلغ بعد الخصم
        public decimal PurchaseAmountPaid { get; set; }
        public decimal PurchaseTotalRemainingAmount { get; set; }
        public decimal PurchaseTotalDiscountAmount { get; set; } // المبلغ المخصوم
        public decimal PurchaseTotalMortgageAmount { get; set; } // قيمة الرهن
        public decimal PurchaseTotalMortgageAmountPaid { get; set; } // قيمة الرهن المدفوع
        public decimal PurchaseTotalRemainingMortgageAmount { get; set; } // قيمة الرهن المتبقي
        public decimal CurrentAmountBalance { get; set; } // Positive = merchant owes us, Negative = we owe merchant
        public decimal CurrentMortgageAmountBalance { get; set; } // Positive = merchant owes us, Negative = we owe merchant
        public decimal SellPrice { get; set; } 
        public decimal SellTotalAmount { get; set; } 
        public decimal SellAmountPaid { get; set; }
        public decimal SellTotalRemainingAmount { get; set; }
        public decimal SellTotalDiscountAmount { get; set; } 
        public decimal SellTotalMortgageAmount { get; set; } 
        public decimal SellTotalMortgageAmountPaid { get; set; } 
        public decimal SellTotalRemainingMortgageAmount { get; set; } 
        public ICollection<MerchantPayment> Payments { get; set; } = new List<MerchantPayment>();
        public ICollection<MerchantTransaction> Transactions { get; set; } = new List<MerchantTransaction>();
    }
}
