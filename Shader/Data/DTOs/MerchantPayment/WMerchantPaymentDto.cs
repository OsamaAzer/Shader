using Shader.Enums;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Shader.Data.DTOs.MerchantPayment
{
    public class WMerchantPaymentDto
    {
        [Required]
        public int MerchantId { get; set; }
        public decimal MortgageAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
