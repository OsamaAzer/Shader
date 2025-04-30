using Shader.Enums;

namespace Shader.Data.DTOs
{
    public class RAllFruitsDTO
    {
        public string FruitName { get; set; }
        public FruitStatus Status { get; set; }
        public int TotalCages { get; set; }
        public int SoldCages { get; set; }
        public int RemainingCages { get; set; }
        public decimal NumberOfKilogramsSold { get; set; }
        public decimal PriceOfKilogramsSold { get; set; }
    }
}
