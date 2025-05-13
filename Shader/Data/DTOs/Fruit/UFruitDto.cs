using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.Fruit
{
    public class UFruitDto
    {
        public string FruitName { get; set; }
        public decimal CageMortgageValue { get; set; } // قيمة رهن القفص
        public int TotalCages { get; set; }
        [Required]
        public int SupplierId { get; set; }
    }
}
