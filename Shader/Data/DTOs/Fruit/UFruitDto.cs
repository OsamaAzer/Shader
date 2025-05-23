using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.Fruit
{
    public class UFruitDto
    {
        [Required]
        public string FruitName { get; set; }
        public decimal CageMortgageValue { get; set; } // قيمة رهن القفص
        [Required]
        public int TotalCages { get; set; }
        [Required]
        public int SupplierId { get; set; }
        public decimal MerchantPurchasePrice { get; set; }
        public decimal MashalValue { get; set; }
        public decimal NylonValue { get; set; }
    }
}
