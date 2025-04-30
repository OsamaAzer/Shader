using Shader.Data.Entities;
using Shader.Enums;

namespace Shader.Data.DTOs
{
    public class RFruitDTO
    {
        public string FruitName { get; set; }
        public FruitStatus Status { get; set; }
        public bool IsCageHasMortgage { get; set; } = false;
        public decimal CageMortgageValue { get; set; } // قيمة رهن القفص
        public int NumberOfMortgagePaidCages { get; set; } // عدد القفص المدفوع الرهن
        public int TotalCages { get; set; }
        public int SoldCages { get; set; }
        public int RemainingCages { get; set; }
    }
}
