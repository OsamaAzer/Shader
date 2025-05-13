using Shader.Data.Entities;
using Shader.Enums;

namespace Shader.Data.Dtos.Fruit
{
    public class RFruitDetailsDto
    {
        public int Id { get; set; }
        public string FruitName { get; set; }
        public FruitStatus Status { get; set; }
        public DateTime Date { get; set; } 
        public string SupplierName { get; set; }
        public decimal MerchantPurchasePrice { get; set; } 
        public bool IsBilled { get; set; }
        public bool IsCageHasMortgage { get; set; }
        public decimal CageMortgageValue { get; set; }
        public int TotalCages { get; set; }
        public int SoldCages { get; set; }
        public int RemainingCages { get; set; }
        public decimal NumberOfKilogramsSold { get; set; }
        public decimal PriceOfKilogramsSold { get; set; }
    }
}
