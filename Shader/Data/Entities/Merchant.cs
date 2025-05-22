using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Merchant
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; }
        public bool IsDeleted { get; set; } = false;
        public decimal PurchasePrice { get; set; } // المبلغ الكلي
        public decimal PurchaseTotalDiscountAmount { get; set; } // المبلغ المخصوم
        public decimal PurchaseTotalAmount => PurchasePrice - PurchaseTotalDiscountAmount; // المبلغ الكلي بعد الخصم
        public decimal PurchaseAmountPaid { get; set; }
        public decimal PurchaseTotalRemainingAmount => PurchaseTotalAmount - PurchaseAmountPaid; // المبلغ المتبقي
        public decimal PurchaseTotalMortgageAmount { get; set; } // قيمة الرهن
        public decimal PurchaseTotalMortgageAmountPaid { get; set; } // قيمة الرهن المدفوع
        public decimal PurchaseTotalRemainingMortgageAmount => PurchaseTotalMortgageAmount - PurchaseTotalMortgageAmountPaid; // المبلغ المتبقي من الرهن
        public decimal CurrentAmountBalance => SellTotalRemainingAmount - PurchaseTotalRemainingAmount; // Positive = merchant owes us, Negative = we owe merchant
        public decimal CurrentMortgageAmountBalance => SellTotalRemainingMortgageAmount - PurchaseTotalRemainingMortgageAmount; // Positive = merchant owes us, Negative = we owe merchant
        public decimal SellPrice { get; set; } 
        public decimal SellTotalDiscountAmount { get; set; }
        public decimal SellTotalAmount => SellPrice - SellTotalDiscountAmount; // المبلغ الكلي بعد الخصم
        public decimal SellAmountPaid { get; set; }
        public decimal SellTotalRemainingAmount => SellTotalAmount - SellAmountPaid; // المبلغ المتبقي
        public decimal SellTotalMortgageAmount { get; set; } 
        public decimal SellTotalMortgageAmountPaid { get; set; } 
        public decimal SellTotalRemainingMortgageAmount => SellTotalMortgageAmount - SellTotalMortgageAmountPaid; // المبلغ المتبقي من الرهن 
        public ICollection<MerchantPayment> Payments { get; set; } = new List<MerchantPayment>();
        public ICollection<MerchantTransaction> Transactions { get; set; } = new List<MerchantTransaction>();
    }
}
