using Shader.Data.Dtos.Fruit;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class FruitProfile
    {
        public static RFruitDetailsDto ToRFruitDetailsDto(this Fruit fruit)
        {
            return new RFruitDetailsDto
            {
                FruitName = fruit.FruitName,
                Status = fruit.Status,
                Date = fruit.Date,
                SupplierName = fruit.Supplier?.Name,
                MerchantPurchasePrice = fruit.MerchantPurchasePrice,
                IsDeleted = fruit.IsDeleted,
                IsBilled = fruit.IsBilled,
                IsCageHasMortgage = fruit.IsCageHasMortgage,
                CageMortgageValue = fruit.CageMortgageValue,
                TotalCages = fruit.TotalCages,
                SoldCages = fruit.SoldCages,
                RemainingCages = fruit.RemainingCages,
                NumberOfKilogramsSold = fruit.NumberOfKilogramsSold,
                PriceOfKilogramsSold = fruit.PriceOfKilogramsSold
            };
        }
    }
}
