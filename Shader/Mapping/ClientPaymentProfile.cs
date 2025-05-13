using Shader.Data.DTOs.ClientPayment;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class ClientPaymentProfile
    {
        public static RClientPaymentDto ToRClientPayment(this ClientPayment payment)
        {
            return new RClientPaymentDto
            {
                ClientName = payment.Client.Name,
                Date = payment.Date,
                MortgageAmount = payment.MortgageAmount,
                PaidAmount = payment.PaidAmount
            };
        }
        public static ClientPayment ToClientPayment(this WClientPaymentDto paymentDto, ClientPayment? payment = null)
        {
            payment ??= new ClientPayment();
            payment.Date = DateTime.Now;
            payment.MortgageAmount = paymentDto.MortgageAmount;
            payment.PaidAmount = paymentDto.PaidAmount;
            payment.ClientId = paymentDto.ClientId;
            return payment;
        }

        public static IEnumerable<RClientPaymentDto> ToRClientPayments(this IEnumerable<ClientPayment> payments)
        {
            return payments.Select(payment => new RClientPaymentDto
            {
                ClientName = payment.Client.Name,
                Date = payment.Date,
                MortgageAmount = payment.MortgageAmount,
                PaidAmount = payment.PaidAmount
            });
        }
    }
}
