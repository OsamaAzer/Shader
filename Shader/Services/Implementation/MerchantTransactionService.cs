using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.ShaderTransaction;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class MerchantTransactionService(ShaderContext context, IFruitService fruitService, IMerchantService merchantService) : IMerchantTransactionService
    {
        public async Task<PagedResponse<RMerchantTDto>> GetAllTransactionsAsync(int pageNumber, int pageSize)
        {
            var transactions = await context.MerchantTransactions
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Merchant)
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
            return transactions.MapToRMerchantTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RMerchantTDto>> GetAllTransactionsByDateAsync(DateOnly date, int pageNumber, int pageSize)
        {
            var transactions = await context.MerchantTransactions
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Merchant)
                .Where(c => DateOnly.FromDateTime(c.Date) == date && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
            return transactions.MapToRMerchantTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RMerchantTDto>> GetAllTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate == default || endDate == default)
                throw new Exception("Start date and end date are both required.");

            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var transactions = await context.MerchantTransactions
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Merchant)
                .Where(c => !c.IsDeleted)
                .Where(c => DateOnly.FromDateTime(c.Date) >= startDate && DateOnly.FromDateTime(c.Date) <= endDate)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return transactions.MapToRMerchantTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RMerchantTDto>> GetTransactionsByMerchantIdAsync(int merchantId, int pageNumber, int pageSize)
        {
            var merchant = await context.Merchants
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == merchantId) ??
                throw new Exception($"The merchant with Id: ({merchantId}) doesn't exist!!");

            var transactions = await context.MerchantTransactions
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Merchant)
                .Where(c => c.MerchantId == merchantId && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
            return transactions.MapToRMerchantTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<RMerchantTDetailsDto> GetTransactionByIdAsync(int id)
        {
            var transaction = await context.MerchantTransactions
                .Where(c => !c.IsDeleted)
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Include(c => c.Merchant)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("This merchant transacttion dosen't exist");
            return transaction.MapToRMerchantTDetailsDto();
        }
        public async Task<RMerchantTDetailsDto> CreateTransactionAsync(WMerchantTDto mtDto)
        {
            var merchant = await context.Merchants
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == mtDto.MerchantId) ??
                throw new Exception($"The merchant with Id:({mtDto.MerchantId}) dosen't exist!!");

            if (mtDto.MerchantTransactionFruits.All(c => c.FruitId == 0)) 
                throw new Exception("You must select a fruit!!");

            var transaction = mtDto.MapToMerchantTransaction();

            foreach (var mtf in mtDto.MerchantTransactionFruits)
            {
                if (mtDto.MerchantTransactionFruits.Where(c => c.FruitId == mtf.FruitId).Count() > 1)
                    throw new Exception("You can't add the same fruit multible times in the same transaction!");

                var fruit = await context.Fruits
                    .Where(f => !f.IsDeleted)
                    .FirstOrDefaultAsync(f => f.Id == mtf.FruitId) ?? 
                    throw new Exception("This fruit dosen't exist");

                fruitService.UpdateTookFruitInMerchantTransaction(fruit, mtf);
                if (fruit.IsCageHasMortgage)
                    transaction.TotalCageMortgageAmount += fruit.CageMortgageValue * mtf.NumberOfCages;
            }
            //merchant.PurchasePrice += transaction.Price;
            //merchant.PurchaseTotalDiscountAmount += transaction.DiscountAmount;
            //merchant.PurchaseTotalAmount += transaction.TotalAmount;
            //merchant.PurchaseTotalRemainingAmount += transaction.TotalAmount;
            //merchant.PurchaseTotalMortgageAmount += transaction.TotalCageMortgageAmount;
            //merchant.PurchaseTotalRemainingMortgageAmount += transaction.TotalCageMortgageAmount;
            //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            merchantService.UpdateMerchantWithIncreaseInTransaction(merchant, transaction);
            await context.MerchantTransactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return transaction.MapToRMerchantTDetailsDto();
        }
        public async Task<RMerchantTDetailsDto> UpdateTransactionAsync(int id, WMerchantTDto mtDto)
        {
            var merchant = await context.Merchants
                .Include(c => c.Transactions)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == mtDto.MerchantId) ?? throw new Exception("This Merchant dosen't exist");

            var transaction = await context.MerchantTransactions
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("This transaction dosen't exist");

            var oldMerchantId = transaction.MerchantId;
            if (oldMerchantId != mtDto.MerchantId)
            {
                var removedMerchant = await context.Merchants
                    .Include(c => c.Transactions)
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == oldMerchantId);

                if (removedMerchant is not null)
                    merchantService.UpdateMerchantWithDecreaseInTransaction(removedMerchant, transaction);
            }
            else
            {
                merchantService.UpdateMerchantWithDecreaseInTransaction(merchant, transaction);
            }
            foreach (var mtfDto in mtDto.MerchantTransactionFruits)
            {
                foreach (var mtf in transaction.MerchantTransactionFruits)
                {
                    bool addedFruitIsExist = mtDto.MerchantTransactionFruits.Any(c => c.FruitId == mtf.FruitId);
                    if (addedFruitIsExist)
                    {
                        var fruit = await context.Fruits
                            .Where(f => !f.IsDeleted)
                            .FirstOrDefaultAsync(f => f.Id == mtfDto.FruitId) ??
                            throw new Exception($"This fruit with id ({mtfDto.FruitId}) dosen't exist");

                        if (fruit is not null)
                        {
                            fruitService.UpdateReturnedFruitInMerchantTransaction(fruit, mtf);
                            if (fruit.IsCageHasMortgage)
                                transaction.TotalCageMortgageAmount -= fruit.CageMortgageValue * mtf.NumberOfCages;

                            fruitService.UpdateTookFruitInMerchantTransaction(fruit, mtfDto);
                            if (fruit.IsCageHasMortgage)
                                transaction.TotalCageMortgageAmount += fruit.CageMortgageValue * mtfDto.NumberOfCages;
                        }
                    }
                    else
                    {
                        var removedFruit = await context.Fruits
                                    .Where(f => !f.IsDeleted)
                                    .FirstOrDefaultAsync(f => f.Id == mtf.FruitId);
                        if (removedFruit is not null)
                        {
                            fruitService.UpdateReturnedFruitInMerchantTransaction(removedFruit, mtf);
                            if (removedFruit.IsCageHasMortgage)
                                transaction.TotalCageMortgageAmount -= removedFruit.CageMortgageValue * mtf.NumberOfCages;
                        }

                        if (mtDto.MerchantTransactionFruits.Where(c => c.FruitId == mtfDto.FruitId).Count() > 1)
                            throw new Exception("You can't add the same fruit multible times in the same transaction!");

                        var newFruit = await context.Fruits
                            .Where(f => !f.IsDeleted)
                            .FirstOrDefaultAsync(f => f.Id == mtfDto.FruitId) ??
                            throw new Exception($"The fruit with id: ({mtfDto.FruitId}) doesn't exist!!");

                        fruitService.UpdateTookFruitInMerchantTransaction(newFruit, mtfDto);
                        if (newFruit.IsCageHasMortgage)
                            transaction.TotalCageMortgageAmount += newFruit.CageMortgageValue * mtfDto.NumberOfCages;
                    }
                }
            }
            transaction = mtDto.MapToMerchantTransaction(transaction);
            context.MerchantTransactions.Update(transaction);
            merchantService.UpdateMerchantWithIncreaseInTransaction(merchant, transaction);
            await context.SaveChangesAsync();
            return transaction.MapToRMerchantTDetailsDto();
        }
        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await context.MerchantTransactions
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("This transaction doesn't exist");

            foreach (var mtf in transaction.MerchantTransactionFruits)
            {
                var fruit = await context.Fruits
                    .Where(f => f.Id == mtf.FruitId)
                    .Where(f => !f.IsDeleted)
                    .FirstOrDefaultAsync();
                if (fruit is not null)
                {
                    fruitService.UpdateReturnedFruitInMerchantTransaction(fruit, mtf);
                    if (fruit.IsCageHasMortgage)
                        transaction.TotalCageMortgageAmount -= fruit.CageMortgageValue * mtf.NumberOfCages;
                }
            }
            var merchant = await context.Merchants
                .Include(c => c.Transactions)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == transaction.MerchantId);

            if (merchant is not null)
            {
                //merchant.PurchasePrice -= transaction.Price;
                //merchant.PurchaseTotalDiscountAmount -= transaction.DiscountAmount;
                //merchant.PurchaseTotalAmount -= transaction.TotalAmount;
                //merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
                //merchant.PurchaseTotalMortgageAmount -= transaction.TotalCageMortgageAmount;
                //merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
                //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
                //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
                //context.Merchants.Update(merchant);
                merchantService.UpdateMerchantWithDecreaseInTransaction(merchant, transaction);
            }

            transaction.IsDeleted = true;
            context.Update(transaction);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
