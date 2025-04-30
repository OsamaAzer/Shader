using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs
{
    public class AFruitDTO
    {
        [Required, MaxLength(100)]
        public string FruitName { get; set; }
        public bool IsCageHasMortgage { get; set; } = false;
        public decimal? CageMortgageValue { get; set; } // قيمة رهن القفص
        public int TotalCages { get; set; }
        public int SupplierId { get; set; }
    }
}
