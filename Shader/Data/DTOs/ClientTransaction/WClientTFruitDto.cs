using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.ClientTransaction
{
    public class WClientTFruitDto
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
