using Shader.Data.Entities;

namespace Shader.Data.Dtos.CashTransaction
{
    public class WCashTFruitDto
    {
        public int FruitId { get; set; }
        public int NumberOfCages { get; set; } 
        public decimal PriceOfKiloGram { get; set; }
        public decimal WeightInKilograms { get; set; } 
    }
}
