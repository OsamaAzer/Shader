using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.ShaderTransaction
{
    public class WMerchantTFruitDto
    {
        [Required]
        public int FruitId { get; set; }
        public int NumberOfCages { get; set; }
        [Required]
        public decimal WeightInKilograms { get; set; }
        [Required]
        public decimal PriceOfKiloGram { get; set; }
    }
}
