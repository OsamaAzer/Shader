using Shader.Enums;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Shader.Data.DTOs.MerchantPayment
{
    public class WMerchantPaymentDto
    {
        [Required]
        public int MerchantId { get; set; }
        [Required]
        public decimal MortgageAmount { get; set; }
        [Required]
        public decimal PaidAmount { get; set; }
    }
}
