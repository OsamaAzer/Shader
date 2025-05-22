using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.ClientPayment
{
    public class WClientPaymentDto
    {
        [Required]
        public int ClientId { get; set; }
        [Required]
        public decimal MortgageAmount { get; set; }
        [Required]
        public decimal PaidAmount { get; set; }
    }
}
