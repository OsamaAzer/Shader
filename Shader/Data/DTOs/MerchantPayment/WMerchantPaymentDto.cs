using Shader.Enums;
using System.Net;

namespace Shader.Data.DTOs.MerchantPayment
{
    public class WMerchantPaymentDto
    {
        public int MerchantId { get; set; }
        public decimal MortgageAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
