using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.Fruit
{
    public class UFruitDto
    {
        public string FruitName { get; set; }
        public bool IsCageHasMortgage { get; set; } = false;
        public decimal CageMortgageValue { get; set; } // قيمة رهن القفص
        public int TotalCages { get; set; }
        public int SupplierId { get; set; }
    }
}
