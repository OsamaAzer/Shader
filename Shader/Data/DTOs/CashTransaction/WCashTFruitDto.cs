using Shader.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.CashTransaction
{
    public class WCashTFruitDto
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
