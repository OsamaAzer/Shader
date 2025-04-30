using Shader.Data.Entities;

namespace Shader.Data.DTOs
{
    public class CashTransactionFruitDTO
    {
        public int FruitId { get; set; }
        public int NumberOfCages { get; set; } 
        public decimal PriceOfKiloGram { get; set; }
        public decimal WeightInKilograms { get; set; } 
    }
}
