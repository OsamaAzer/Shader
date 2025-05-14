using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.Fruit
{
    public class WRangeFruitDto
    {
            [Required]
            public string FruitName { get; set; }
            public decimal CageMortgageValue { get; set; } // قيمة رهن القفص
            [Required]
            public int TotalCages { get; set; }
            public decimal MerchantPurchasePrice { get; set; }
    }
}
