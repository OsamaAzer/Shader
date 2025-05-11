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
        public decimal SellingPrice { get; set; } 
        public decimal SellingTotalAmount { get; set; } 
        public decimal SellingAmountPaid { get; set; }
        public decimal SellingTotalRemainingAmount { get; set; }
        public decimal SellingTotalDiscountAmount { get; set; } 
        public decimal SellingTotalMortgageAmount { get; set; } 
        public decimal SellingTotalMortgageAmountPaid { get; set; } 
        public decimal SellingTotalRemainingMortgageAmount { get; set; } 
        public ICollection<MerchantPayments> Payments { get; set; } = new List<MerchantPayments>();
        public ICollection<MerchantTransaction> Transactions { get; set; } = new List<MerchantTransaction>();
    }
}
