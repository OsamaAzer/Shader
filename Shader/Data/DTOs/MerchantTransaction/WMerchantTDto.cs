using Shader.Data.Dtos.ClientTransaction;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.ShaderTransaction
{
    public class WMerchantTDto
    {
        public string? Description { get; set; }
        [Required]
        public int ShaderId { get; set; }
        public List<WMerchantTFruitDto> MerchantTransactionFruits { get; set; }
    }
}
