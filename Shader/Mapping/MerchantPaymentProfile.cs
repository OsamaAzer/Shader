using Shader.Data.DTOs.MerchantPayment;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class MerchantPaymentProfile
    {
        public static RMerchantPaymentDto ToRMerchantPayment(this MerchantPayment payment)
        {
            return new RMerchantPaymentDto
            {
                Id = payment.Id,
                MerchantName = payment.Merchant.Name,
                TransactionType = payment.TransactionType,
                Date = payment.Date,
                MortgageAmount = payment.MortgageAmount,
                PaidAmount = payment.PaidAmount
            };
        }
        public static MerchantPayment ToMerchantPayment(this WMerchantPaymentDto paymentDto, MerchantPayment? payment = null)
        {
            payment ??= new MerchantPayment();
            payment.Date = DateTime.Now;
            payment.MortgageAmount = paymentDto.MortgageAmount;
            payment.PaidAmount = paymentDto.PaidAmount;
            payment.MerchantId = paymentDto.MerchantId;
            return payment;
        }

        public static IEnumerable<RMerchantPaymentDto> ToRMerchantPayments(this IEnumerable<MerchantPayment> payments)
        {
            return payments.Select(payment => new RMerchantPaymentDto
            {
                Id = payment.Id,
                MerchantName = payment.Merchant.Name,
                TransactionType = payment.TransactionType,
                Date = payment.Date,
                MortgageAmount = payment.MortgageAmount,
                PaidAmount = payment.PaidAmount
            });
        }
    }
}
