using Shader.Data.Dtos.ClientTransaction;
using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.ShaderTransaction
{
    public class WMerchantTDto
    {
        public string? Description { get; set; }
        public decimal DiscountAmount { get; set; }
        [Required]
        public int MerchantId { get; set; }
        [Required]
        public List<WMerchantTFruitDto> MerchantTransactionFruits { get; set; }
    }
}
