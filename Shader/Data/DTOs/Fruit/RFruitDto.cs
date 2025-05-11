using Shader.Data.Entities;
using Shader.Enums;

namespace Shader.Data.Dtos.Fruit
{
    public class RFruitDto
    {
        public string FruitName { get; set; }
        public FruitStatus Status { get; set; }
        public bool IsCageHasMortgage { get; set; } = false;
        public decimal CageMortgageValue { get; set; } // قيمة رهن القفص
        public int TotalCages { get; set; }
        public int SoldCages { get; set; }
        public int RemainingCages { get; set; }
    }
}
