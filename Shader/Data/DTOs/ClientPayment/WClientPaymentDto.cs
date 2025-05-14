using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.ClientPayment
{
    public class WClientPaymentDto
    {
        [Required]
        public int ClientId { get; set; }
        public decimal MortgageAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
