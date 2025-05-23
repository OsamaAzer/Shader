using Humanizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.CashTransaction;
using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.Dtos.Fruit;
using Shader.Data.DTOs.ShaderTransaction;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class FruitService(ShaderContext context) : IFruitService
    {
        public async Task<PagedResponse<RFruitsDto>> GetAllFruitsAsync(int pageNumber, int pageSize)
        {
            var fruits = await context.Fruits
                .Where(f => !f.IsDeleted)
                .ToListAsync();

            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> GetSupplierFruitsToBeBilledAsync(int supplierId, int pageNumber, int pageSize)
        {
            var supplier = await context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == supplierId && !s.IsDeleted) ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = await context.Fruits
                .Where(f => f.SupplierId == supplierId && !f.Supplier.IsMerchant && !f.IsBilled && !f.IsDeleted && f.Status == FruitStatus.NotAvailabe)
                .ToListAsync();

            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> GetInStockFruitsAsync(int pageNumber, int pageSize)
        {
            var fruits = await context.Fruits
                .Where(f => f.Status == FruitStatus.InStock && !f.IsDeleted)
                .OrderByDescending(f => f.RemainingCages)
                .ToListAsync();

            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> GetInStockSupplierFruitsAsync(int supplierId, int pageNumber, int pageSize)
        {
            var supplier = await context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == supplierId && !s.IsDeleted) ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = await context.Fruits
                .Where(f => f.SupplierId == supplierId && f.Status == FruitStatus.InStock && !f.IsDeleted)
                .OrderByDescending(f => f.RemainingCages)
                .ToListAsync();

            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> GetUnAvailableFruitsAsync(int pageNumber, int pageSize)
        {
            var fruits = await context.Fruits
                .Where(f => f.Status == FruitStatus.NotAvailabe && !f.IsDeleted)
                .ToListAsync();

            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> GetAllSupplierFruitsAsync(int supplierId, int pageNumber, int pageSize)
        {
            var supplier = await context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == supplierId && !s.IsDeleted) ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = await context.Fruits
                .Where(f => f.SupplierId == supplierId && !f.IsDeleted)
                .ToListAsync();

            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> SearchWithFruitNameAsync(string fruitName, int pageNumber, int pageSize)
        {
            var fruits = await context.Fruits
                .Where(f => f.FruitName.ToLower().Contains(fruitName.ToLower()) && !f.IsDeleted)
                .ToListAsync();

            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<RFruitDetailsDto> GetFruitByIdAsync(int id)
        {
            var fruit = await context.Fruits
                .Include(f => f.Supplier)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted) ??
                throw new Exception($"Fruit with id:({id}) does not exist!");

            return fruit.ToRFruitDetailsDto();
        }
        public async Task<RFruitDetailsDto> AddFruitCagesAsync(int id, int numberOfCages)
        {
            var fruit = await context.Fruits
                .Include(f => f.Supplier)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted) ?? 
                throw new Exception($"Fruit with id:({id}) does not exist!");

            if (numberOfCages <= 0) throw new Exception("Number of cages must be greater than 0!");

            fruit.TotalCages += numberOfCages;
            fruit.RemainingCages += numberOfCages;

            if (fruit.TotalCages > 0 && fruit.RemainingCages > 0)
                fruit.Status = FruitStatus.InStock;
            else
                fruit.Status = FruitStatus.NotAvailabe;

            context.Fruits.Update(fruit);
            await context.SaveChangesAsync();
            return fruit.ToRFruitDetailsDto();
        }
        
        public async Task<IEnumerable<RFruitsDto>> AddFruitsAsync(int supplierId, List<WRangeFruitDto> fruitDtos)
        {
            var supplier = await context.Suppliers
                .Include(s => s.Merchant)
                .FirstOrDefaultAsync(s => s.Id == supplierId && !s.IsDeleted) ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = fruitDtos.Map<WRangeFruitDto, Fruit>().ToList();
            foreach (var fruit in fruits)
            {
                if (await context.Fruits.Where(f => !f.IsDeleted).AnyAsync(f => f.FruitName.ToLower() == fruit.FruitName.ToLower()))
                    throw new Exception($"There is a fruit with the same Name : ({fruit.FruitName})!!");
                if (fruit.TotalCages <= 0)
                    throw new Exception($"Total cages must be greater than zero!!");
                if (fruit.CageMortgageValue < 0)
                    throw new Exception($"Please enter a valid Cage mortgage amound");
                if (fruit.MerchantPurchasePrice < 0)
                    throw new Exception($"Please enter a valid Merchant purchase price");

                if (fruit.CageMortgageValue == 0)
                    fruit.IsCageHasMortgage = false;

                if (fruit.CageMortgageValue > 0)
                    fruit.IsCageHasMortgage = true;

                fruit.Date = DateTime.Now;
                fruit.SupplierId = supplierId;
                fruit.SoldCages = 0;
                fruit.RemainingCages = fruit.TotalCages;

                if (fruit.TotalCages > 0 && fruit.RemainingCages > 0)
                    fruit.Status = FruitStatus.InStock;
                else
                    fruit.Status = FruitStatus.NotAvailabe;

                if (!supplier.IsMerchant)
                    fruit.MerchantPurchasePrice = 0;

                if (supplier.IsMerchant && supplier.Merchant is not null)
                {
                    supplier.Merchant.SellPrice += fruit.MerchantPurchasePrice;
                    //supplier.Merchant.SellTotalAmount += fruit.MerchantPurchasePrice;
                    //supplier.Merchant.SellTotalRemainingAmount = supplier.Merchant.SellTotalAmount - supplier.Merchant.SellAmountPaid;
                    //supplier.Merchant.CurrentAmountBalance = 
                    //supplier.Merchant.SellTotalRemainingAmount - supplier.Merchant.PurchaseTotalRemainingAmount;
                    context.Merchants.Update(supplier.Merchant);
                }
            }
            await context.Fruits.AddRangeAsync(fruits);
            await context.SaveChangesAsync();
            return fruits.Map<Fruit, RFruitsDto>();
        }
        public async Task<RFruitDetailsDto> UpdateFruitAsync(int id, UFruitDto dto)
        {
            var fruit = await context.Fruits
                .Include(f => f.Supplier)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted) ?? 
                throw new Exception($"Fruit with id:({id}) does not exist!");

            var supplier = await context.Suppliers
                    .Include(s => s.Merchant)
                    .FirstOrDefaultAsync(f => f.Id == dto.SupplierId && !f.IsDeleted) ??
                    throw new Exception($"Supplier with id:({dto.SupplierId}) does not exist!");

            if (dto.TotalCages <= 0)
                throw new Exception($"Please Enter a valid number of cages!!");
            if (dto.CageMortgageValue < 0)
                throw new Exception($"Please enter a valid Cage mortgage amound");
            if (dto.MerchantPurchasePrice < 0)
                throw new Exception($"Please enter a valid Merchant purchase price");

            var nameFlag = context.Fruits
                .Where(f => !f.IsDeleted && f.FruitName.ToLower() == dto.FruitName.ToLower())
                .Count() >= 1 && fruit.FruitName.ToLower() != dto.FruitName.ToLower();

            if (nameFlag) throw new Exception($"Fruit with name {dto.FruitName} already exists!!");

            if (fruit.SupplierId != dto.SupplierId)
            {
                var oldSupplier = await context.Suppliers
                .Include(s => s.Merchant)
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == fruit.SupplierId);

                if (oldSupplier is not null && oldSupplier.IsMerchant && oldSupplier.Merchant is not null)
                {
                    oldSupplier.Merchant.SellPrice -= fruit.MerchantPurchasePrice;
                    //oldSupplier.Merchant.SellTotalAmount -= fruit.MerchantPurchasePrice;
                    //oldSupplier.Merchant.SellTotalRemainingAmount = oldSupplier.Merchant.SellTotalAmount - oldSupplier.Merchant.SellAmountPaid;
                    //oldSupplier.Merchant.CurrentAmountBalance =
                    //oldSupplier.Merchant.SellTotalRemainingAmount - oldSupplier.Merchant.PurchaseTotalRemainingAmount;
                    context.Merchants.Update(oldSupplier.Merchant);
                }
            }
            if (supplier.IsMerchant && supplier.Merchant is not null)
            {
                supplier.Merchant.SellPrice -= fruit.MerchantPurchasePrice;
                //supplier.Merchant.SellTotalAmount -= fruit.MerchantPurchasePrice;
                //supplier.Merchant.SellTotalRemainingAmount = supplier.Merchant.SellTotalAmount - supplier.Merchant.SellAmountPaid;
                //supplier.Merchant.CurrentAmountBalance =
                //supplier.Merchant.SellTotalRemainingAmount - supplier.Merchant.PurchaseTotalRemainingAmount;

                dto.Map(fruit);

                supplier.Merchant.SellPrice += fruit.MerchantPurchasePrice;
                //supplier.Merchant.SellTotalAmount += fruit.MerchantPurchasePrice;
                //supplier.Merchant.SellTotalRemainingAmount = supplier.Merchant.SellTotalAmount - supplier.Merchant.SellAmountPaid;
                //supplier.Merchant.CurrentAmountBalance =
                //supplier.Merchant.SellTotalRemainingAmount - supplier.Merchant.PurchaseTotalRemainingAmount;
                context.Merchants.Update(supplier.Merchant);
            }
            if (!supplier.IsMerchant)
                fruit.MerchantPurchasePrice = 0;

                //dto.Map(fruit);
            fruit.SupplierId = supplier.Id;
            fruit.Supplier = supplier;
            fruit.RemainingCages = fruit.TotalCages - fruit.SoldCages;

            if (fruit.CageMortgageValue == 0)
                fruit.IsCageHasMortgage = false;

            if (fruit.CageMortgageValue > 0)
                fruit.IsCageHasMortgage = true;

            if (fruit.RemainingCages > 0 && fruit.RemainingCages > 0)
                fruit.Status = FruitStatus.InStock;
            else
                fruit.Status = FruitStatus.NotAvailabe;

            // todo : check if the fruit has a bill or not
            // todo : check the transactions mortgage values have been done on the fruit if the mortgage amount changed or unchecked!

            context.Fruits.Update(fruit);
            await context.SaveChangesAsync();
            return fruit.ToRFruitDetailsDto();
        }

        public async Task<bool> DeleteFruitAsync(int id)
        {
            var fruit = await context.Fruits
                .Include(f => f.Supplier)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted) ??
                throw new Exception($"Fruit with id:({id}) does not exist!");

            var supplier = await context.Suppliers
                .Include(s => s.Merchant)
                .FirstOrDefaultAsync(s => s.Id == fruit.SupplierId && !s.IsDeleted);

            if (fruit.RemainingCages > 0)
                throw new Exception($"There is still a stock of fruit in the store.");

            fruit.IsDeleted = true;
            context.Fruits.Update(fruit);
            return await context.SaveChangesAsync() > 0;
        }

        public Fruit UpdateTookFruitInClientTransaction(Fruit fruit, WClientTFruitDto ctfDto)
        {
            if (ctfDto.NumberOfCages <= 0)
                throw new Exception("The number of cages must be greater than Zero");
            if (ctfDto.NumberOfCages > fruit.RemainingCages)
                throw new Exception($"the available cages of ({fruit.FruitName}) equal {fruit.RemainingCages}");
            if (ctfDto.WeightInKilograms <= 0)
                throw new Exception("The Weight must be greater than Zero");
            if (ctfDto.PriceOfKiloGram <= 0)
                throw new Exception("The price of kilogrammust be greater than Zero");
            if (fruit.RemainingCages == 0 && fruit.Status == FruitStatus.NotAvailabe)
                throw new Exception("The number of cages is not enough");

            fruit.NumberOfKilogramsSold += ctfDto.WeightInKilograms;
            fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
            fruit.PriceOfKilogramsSold += ctfDto.PriceOfKiloGram * ctfDto.WeightInKilograms;
            fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
            fruit.RemainingCages -= ctfDto.NumberOfCages;
            fruit.SoldCages += ctfDto.NumberOfCages;

            if (fruit.RemainingCages == 0 && fruit.SoldCages == fruit.TotalCages)
                fruit.Status = FruitStatus.NotAvailabe;
            else
                fruit.Status = FruitStatus.InStock;

            context.Fruits.Update(fruit);
            return fruit;
        }
        public Fruit UpdateReturnedFruitInClientTransaction(Fruit fruit, WClientTFruitDto ctf)
        {
            fruit.NumberOfKilogramsSold -= ctf.WeightInKilograms;
            fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
            fruit.PriceOfKilogramsSold -= ctf.PriceOfKiloGram * ctf.WeightInKilograms;
            fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
            fruit.RemainingCages += ctf.NumberOfCages;
            fruit.SoldCages -= ctf.NumberOfCages;

            if (fruit.RemainingCages == 0 && fruit.SoldCages == fruit.TotalCages)
                fruit.Status = FruitStatus.NotAvailabe;
            else
                fruit.Status = FruitStatus.InStock;

            context.Fruits.Update(fruit);
            return fruit;
        }
        public Fruit UpdateReturnedFruitInClientTransaction(Fruit fruit, ClientTransactionFruit ctf)
        {
            fruit.NumberOfKilogramsSold -= ctf.WeightInKilograms;
            fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
            fruit.PriceOfKilogramsSold -= ctf.PriceOfKiloGram * ctf.WeightInKilograms;
            fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
            fruit.RemainingCages += ctf.NumberOfCages;
            fruit.SoldCages -= ctf.NumberOfCages;

            if (fruit.RemainingCages == 0 && fruit.SoldCages == fruit.TotalCages)
                fruit.Status = FruitStatus.NotAvailabe;
            else
                fruit.Status = FruitStatus.InStock;

            context.Fruits.Update(fruit);
            return fruit;
        }
        public Fruit UpdateTookFruitInCashTransaction(Fruit fruit, WCashTFruitDto ctfDto)
        {
            if (ctfDto.NumberOfCages <= 0)
                throw new Exception("The number of cages must be greater than Zero");
            if (ctfDto.NumberOfCages > fruit.RemainingCages)
                throw new Exception($"the available cages of ({fruit.FruitName}) equal {fruit.RemainingCages}");
            if (ctfDto.WeightInKilograms <= 0)
                throw new Exception("The Weight must be greater than Zero");
            if (ctfDto.PriceOfKiloGram <= 0)
                throw new Exception("The price of kilograms must be greater than Zero");
            if (fruit.RemainingCages == 0 && fruit.Status == FruitStatus.NotAvailabe)
                throw new Exception($"This fruit with id: ({ctfDto.FruitId}) is not available");
            if ((fruit.RemainingCages == 0) && (fruit.SoldCages == fruit.TotalCages) && (fruit.Status == FruitStatus.NotAvailabe))
                throw new Exception($"This fruit with id: ({ctfDto.FruitId}) is not available");

            fruit.NumberOfKilogramsSold += ctfDto.WeightInKilograms;
            fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
            fruit.PriceOfKilogramsSold += ctfDto.PriceOfKiloGram * ctfDto.WeightInKilograms;
            fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
            fruit.RemainingCages -= ctfDto.NumberOfCages;
            fruit.SoldCages += ctfDto.NumberOfCages;

            if (fruit.RemainingCages == 0 && fruit.SoldCages == fruit.TotalCages)
                fruit.Status = FruitStatus.NotAvailabe;
            else
                fruit.Status = FruitStatus.InStock;

            context.Fruits.Update(fruit);
            return fruit;
        }
        public Fruit UpdateReturnedFruitInCashTransaction(Fruit fruitEntity, CashTransactionFruit ctf)
        {
            fruitEntity.NumberOfKilogramsSold -= ctf.WeightInKilograms;
            fruitEntity.PriceOfKilogramsSold -= ctf.PriceOfKiloGram * ctf.WeightInKilograms;
            fruitEntity.PriceOfKilogramsSold = Math.Round(fruitEntity.PriceOfKilogramsSold, 2);
            fruitEntity.RemainingCages += ctf.NumberOfCages;
            fruitEntity.SoldCages -= ctf.NumberOfCages;

            if (fruitEntity.RemainingCages == 0 && fruitEntity.SoldCages == fruitEntity.TotalCages)
                fruitEntity.Status = FruitStatus.NotAvailabe;
            else
                fruitEntity.Status = FruitStatus.InStock;

            context.Fruits.Update(fruitEntity);
            return fruitEntity;
        }
        public Fruit UpdateTookFruitInMerchantTransaction(Fruit fruit, WMerchantTFruitDto mtf)
        {
            if (mtf.NumberOfCages <= 0)
                throw new Exception("The number of cages must be greater than Zero");
            if (mtf.NumberOfCages > fruit.RemainingCages)
                throw new Exception($"the available cages of ({fruit.FruitName}) equal {fruit.RemainingCages}");
            if (mtf.WeightInKilograms <= 0)
                throw new Exception("The Weight must be greater than Zero");
            if (mtf.PriceOfKiloGram <= 0)
                throw new Exception("The price of kilogrammust be greater than Zero");
            if (fruit.RemainingCages == 0 && fruit.Status == FruitStatus.NotAvailabe)
                throw new Exception("The isn't enough stock of fruit!"); 

            fruit.NumberOfKilogramsSold += mtf.WeightInKilograms;
            fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
            fruit.PriceOfKilogramsSold += mtf.PriceOfKiloGram * mtf.WeightInKilograms;
            fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
            fruit.RemainingCages -= mtf.NumberOfCages;
            fruit.SoldCages += mtf.NumberOfCages;

            if (fruit.RemainingCages == 0 && fruit.SoldCages == fruit.TotalCages)
                fruit.Status = FruitStatus.NotAvailabe;
            else
                fruit.Status = FruitStatus.InStock;

            context.Fruits.Update(fruit);
            return fruit;
        }
        public Fruit UpdateReturnedFruitInMerchantTransaction(Fruit removedFruit, MerchantTransactionFruit mtf)
        {
            removedFruit.NumberOfKilogramsSold -= mtf.WeightInKilograms;
            removedFruit.NumberOfKilogramsSold = Math.Round(removedFruit.NumberOfKilogramsSold, 2);
            removedFruit.PriceOfKilogramsSold -= mtf.PriceOfKiloGram * mtf.WeightInKilograms;
            removedFruit.PriceOfKilogramsSold = Math.Round(removedFruit.PriceOfKilogramsSold, 2);
            removedFruit.RemainingCages += mtf.NumberOfCages;
            removedFruit.SoldCages -= mtf.NumberOfCages;

            if (removedFruit.RemainingCages == 0 && removedFruit.SoldCages == removedFruit.TotalCages)
                removedFruit.Status = FruitStatus.NotAvailabe;
            else
                removedFruit.Status = FruitStatus.InStock;

            context.Fruits.Update(removedFruit);
            return removedFruit;
        }

        
        
    }
}
