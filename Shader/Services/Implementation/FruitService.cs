using Humanizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.Fruit;
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
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == supplierId) ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = await context.Fruits
                .Where(f => f.SupplierId == supplierId && !f.Supplier.IsMerchant)
                .Where(f => !f.IsBilled && !f.IsDeleted)
                .Where(f => f.Status == FruitStatus.NotAvailabe)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> GetInStockFruitsAsync(int pageNumber, int pageSize)
        {
            var fruits = await context.Fruits
                .Where(f => f.Status == FruitStatus.InStock)
                .Where(f => !f.IsDeleted)
                .OrderByDescending(f => f.RemainingCages)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> GetInStockSupplierFruitsAsync(int supplierId, int pageNumber, int pageSize)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == supplierId && !s.IsDeleted)
                .FirstOrDefaultAsync() ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = await context.Fruits
                .Where(f => f.SupplierId == supplierId)
                .Where(f => f.Status == FruitStatus.InStock)
                .Where (f => !f.IsDeleted)
                .OrderByDescending(f => f.RemainingCages)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> GetUnAvailableFruitsAsync(int pageNumber, int pageSize)
        {
            var fruits = await context.Fruits
                .Where(f => f.Status == FruitStatus.NotAvailabe)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> GetAllSupplierFruitsAsync(int supplierId, int pageNumber, int pageSize)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == supplierId && !s.IsDeleted)
                .FirstOrDefaultAsync() ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = await context.Fruits
                .Where(f => f.SupplierId == supplierId)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RFruitsDto>> SearchWithFruitNameAsync(string fruitName, int pageNumber, int pageSize)
        {
            var fruits = await context.Fruits
                .Where(f => f.FruitName.ToLower().Contains(fruitName.ToLower()))
                .Where(f => !f.IsDeleted)
                .ToListAsync();

            return await Task.FromResult(fruits.Map<Fruit, RFruitsDto>().CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<RFruitDetailsDto> GetFruitByIdAsync(int id)
        {
            var fruit = await context.Fruits
                .Include(f => f.Supplier)
                .Where(f => f.Id == id && !f.IsDeleted)
                .FirstOrDefaultAsync() ??
                throw new Exception($"Fruit with id:({id}) does not exist!");

            return fruit.ToRFruitDetailsDto();
        }
        public async Task<RFruitDetailsDto> AddFruitCagesAsync(int id, int numberOfCages)
        {
            var fruit = await context.Fruits
                .Include(f => f.Supplier)
                .Where(s => s.Id == id && !s.IsDeleted)
                .FirstOrDefaultAsync() ?? throw new Exception($"Fruit with id:({id}) does not exist!");

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
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == supplierId)??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = fruitDtos.Map<WRangeFruitDto, Fruit>().ToList();
            foreach (var fruit in fruits)
            {
                if (context.Fruits.Where(f => !f.IsDeleted).Any(f => f.FruitName == fruit.FruitName))
                    throw new Exception($"There is a fruit with the same Name!!");

                if (fruit.TotalCages <= 0)
                    throw new Exception($"Please Enter a valid number of cages!!");

                if (fruit.CageMortgageValue < 0)
                    throw new Exception($"Please enter a valid Cage mortgage amound");

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
                    supplier.Merchant.SellTotalAmount += fruit.MerchantPurchasePrice;
                    supplier.Merchant.SellTotalRemainingAmount = supplier.Merchant.SellTotalAmount - supplier.Merchant.SellAmountPaid;
                    supplier.Merchant.CurrentAmountBalance = 
                        supplier.Merchant.SellTotalRemainingAmount - supplier.Merchant.PurchaseTotalRemainingAmount;
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
                .Where(f => !f.IsDeleted)
                .FirstOrDefaultAsync(f => f.Id == id) ?? 
                throw new Exception($"Fruit with id:({id}) does not exist!");

            var supplier = await context.Suppliers
                    .Include(s => s.Merchant)
                    .Where(f => !f.IsDeleted)
                    .FirstOrDefaultAsync(f => f.Id == dto.SupplierId) ??
                    throw new Exception($"Supplier with id:({dto.SupplierId}) does not exist!");

            if (dto.TotalCages <= 0)
                throw new Exception($"Please Enter a valid number of cages!!");

            if (dto.CageMortgageValue < 0)
                throw new Exception($"Please enter a valid Cage mortgage amound");

            if (context.Fruits.Where(f => !f.IsDeleted && f.FruitName == fruit.FruitName).Count() >= 1) // TODO : Handle Repeated fruit names
                throw new Exception($"There is a fruit with the same Name!!");

            if (fruit.SupplierId != dto.SupplierId)
            {
                var oldSupplier = await context.Suppliers
                .Include(s => s.Merchant)
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == fruit.SupplierId);

                if (oldSupplier is not null && oldSupplier.IsMerchant && oldSupplier.Merchant is not null)
                {
                    oldSupplier.Merchant.SellPrice -= fruit.MerchantPurchasePrice;
                    oldSupplier.Merchant.SellTotalAmount -= fruit.MerchantPurchasePrice;
                    oldSupplier.Merchant.SellTotalRemainingAmount = oldSupplier.Merchant.SellTotalAmount - oldSupplier.Merchant.SellAmountPaid;
                    oldSupplier.Merchant.CurrentAmountBalance =
                    oldSupplier.Merchant.SellTotalRemainingAmount - oldSupplier.Merchant.PurchaseTotalRemainingAmount;
                    context.Merchants.Update(oldSupplier.Merchant);
                }
            }
            if (supplier.IsMerchant && supplier.Merchant is not null)
            {
                supplier.Merchant.SellPrice -= fruit.MerchantPurchasePrice;
                supplier.Merchant.SellTotalAmount -= fruit.MerchantPurchasePrice;
                supplier.Merchant.SellTotalRemainingAmount = supplier.Merchant.SellTotalAmount - supplier.Merchant.SellAmountPaid;
                supplier.Merchant.CurrentAmountBalance =
                supplier.Merchant.SellTotalRemainingAmount - supplier.Merchant.PurchaseTotalRemainingAmount;

                dto.Map(fruit);

                supplier.Merchant.SellPrice += fruit.MerchantPurchasePrice;
                supplier.Merchant.SellTotalAmount += fruit.MerchantPurchasePrice;
                supplier.Merchant.SellTotalRemainingAmount = supplier.Merchant.SellTotalAmount - supplier.Merchant.SellAmountPaid;
                supplier.Merchant.CurrentAmountBalance =
                supplier.Merchant.SellTotalRemainingAmount - supplier.Merchant.PurchaseTotalRemainingAmount;
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
                .Where(f => !f.IsDeleted )
                .FirstOrDefaultAsync(f => f.Id == id) ??
                throw new Exception($"Fruit with id:({id}) does not exist!");

            //var supplier = await context.Suppliers
            //    .Include(s => s.Merchant)
            //    .Where(s => !s.IsDeleted)
            //    .FirstOrDefaultAsync(s => s.Id == fruit.SupplierId);

            if (fruit.RemainingCages > 0)
                throw new Exception($"There is still a stock of fruit in the store.");

            fruit.IsDeleted = true;
            context.Fruits.Update(fruit);
            return await context.SaveChangesAsync() > 0;
        }
        
    }
}
