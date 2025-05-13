using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.Fruit
{
    public class WRangeFruitDto
    {
            [Required, MaxLength(100)]
            public string FruitName { get; set; }
            public bool IsCageHasMortgage { get; set; } = false;
            public decimal? CageMortgageValue { get; set; } // قيمة رهن القفص
            public int TotalCages { get; set; }
            public decimal MerchantPurchasePrice { get; set; }
    }
}
