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
    public class MerchantTransactionService(ShaderContext context) : IMerchantTransactionService
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

                if (mtf.NumberOfCages <= 0)
                    throw new Exception("The number of cages must be greater than Zero");
                if (mtf.WeightInKilograms <= 0)
                    throw new Exception("The Weight must be greater than Zero");
                if (mtf.PriceOfKiloGram <= 0)
                    throw new Exception("The price of kilogrammust be greater than Zero");
                if (fruit.RemainingCages == 0 && fruit.Status == FruitStatus.NotAvailabe)
                    throw new Exception("The number of cages is not enough");

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

                if (fruit.IsCageHasMortgage)
                {
                    transaction.TotalCageMortgageAmount += fruit.CageMortgageValue * mtf.NumberOfCages;
                }
                context.Fruits.Update(fruit);
            }
            merchant.PurchasePrice += transaction.Price;
            merchant.PurchaseTotalDiscountAmount += transaction.DiscountAmount;
            merchant.PurchaseTotalAmount += transaction.TotalAmount;
            merchant.PurchaseTotalRemainingAmount += transaction.TotalAmount;
            merchant.PurchaseTotalMortgageAmount += transaction.TotalCageMortgageAmount;
            merchant.PurchaseTotalRemainingMortgageAmount += transaction.TotalCageMortgageAmount;
            merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            await context.MerchantTransactions.AddAsync(transaction);
            context.Merchants.Update(merchant);
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

            foreach (var mtf in transaction.MerchantTransactionFruits)
            {
                bool flag = mtDto.MerchantTransactionFruits.Any(c => c.FruitId == mtf.FruitId);
                if (!flag)
                {
                    var removedFruit = await context.Fruits
                        .Where(f => f.Id == mtf.FruitId)
                        .Where(f => !f.IsDeleted)
                        .FirstOrDefaultAsync();
                    if (removedFruit is not null)
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

                        if (removedFruit.IsCageHasMortgage)
                        {
                            transaction.TotalCageMortgageAmount -= removedFruit.CageMortgageValue * mtf.NumberOfCages;
                        }
                        context.Fruits.Update(removedFruit);
                    }
                }
                else
                {
                    foreach (var ctfDto in mtDto.MerchantTransactionFruits)
                    {
                        if (mtDto.MerchantTransactionFruits.Where(c => c.FruitId == ctfDto.FruitId).Count() > 1)
                            throw new Exception("You can't add the same fruit multible times in the same transaction!");

                        var fruit = await context.Fruits
                            .Where(f => !f.IsDeleted)
                            .FirstOrDefaultAsync(f => f.Id == ctfDto.FruitId);

                        if (mtf.NumberOfCages <= 0)
                            throw new Exception("The number of cages must be greater than Zero");
                        if (mtf.WeightInKilograms <= 0)
                            throw new Exception("The Weight must be greater than Zero");
                        if (mtf.PriceOfKiloGram <= 0)
                            throw new Exception("The price of kilogrammust be greater than Zero");

                        if (fruit is not null)
                        {
                            if (fruit.RemainingCages == 0) throw new Exception("The number of cages is not enough");
                            fruit.NumberOfKilogramsSold -= mtf.WeightInKilograms;
                            fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
                            fruit.PriceOfKilogramsSold -= mtf.PriceOfKiloGram * mtf.WeightInKilograms;
                            fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
                            fruit.RemainingCages += mtf.NumberOfCages;
                            fruit.SoldCages -= mtf.NumberOfCages;

                            if (fruit.RemainingCages == 0 && fruit.SoldCages == fruit.TotalCages)
                                fruit.Status = FruitStatus.NotAvailabe;
                            else
                                fruit.Status = FruitStatus.InStock;

                            if (fruit.IsCageHasMortgage)
                            {
                                transaction.TotalCageMortgageAmount -= fruit.CageMortgageValue * mtf.NumberOfCages;
                            }

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

                            if (fruit.IsCageHasMortgage)
                            {
                                transaction.TotalCageMortgageAmount = fruit.CageMortgageValue * ctfDto.NumberOfCages;
                            }
                            context.Fruits.Update(fruit);
                        }
                    }
                }
            }
            var oldMerchantId = transaction.MerchantId;
            if (oldMerchantId == mtDto.MerchantId)
            {
                merchant.PurchasePrice -= transaction.Price;
                merchant.PurchaseTotalDiscountAmount -= transaction.DiscountAmount;
                merchant.PurchaseTotalAmount -= transaction.TotalAmount;
                merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
                merchant.PurchaseTotalMortgageAmount -= transaction.TotalCageMortgageAmount;
                merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
                merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
                merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            }

            if (oldMerchantId != mtDto.MerchantId)
            {
                var removedMerchant = await context.Merchants
                    .Include(c => c.Transactions)
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == oldMerchantId);

                if (removedMerchant is not null)
                {
                    removedMerchant.PurchasePrice -= transaction.Price;
                    removedMerchant.PurchaseTotalDiscountAmount -= transaction.DiscountAmount;
                    removedMerchant.PurchaseTotalAmount -= transaction.TotalAmount;
                    removedMerchant.PurchaseTotalRemainingAmount = removedMerchant.PurchaseTotalAmount - removedMerchant.PurchaseAmountPaid;
                    removedMerchant.PurchaseTotalMortgageAmount -= transaction.TotalCageMortgageAmount;
                    removedMerchant.PurchaseTotalRemainingMortgageAmount = removedMerchant.PurchaseTotalMortgageAmount - removedMerchant.PurchaseTotalMortgageAmountPaid;
                    removedMerchant.CurrentAmountBalance = removedMerchant.SellTotalRemainingAmount - removedMerchant.PurchaseTotalRemainingAmount;
                    removedMerchant.CurrentMortgageAmountBalance = removedMerchant.SellTotalRemainingMortgageAmount - removedMerchant.PurchaseTotalRemainingMortgageAmount;
                    context.Merchants.Update(removedMerchant);
                }
            }
            transaction = mtDto.MapToMerchantTransaction(transaction);
            context.MerchantTransactions.Update(transaction);
            merchant.PurchasePrice += transaction.Price;
            merchant.PurchaseTotalDiscountAmount += transaction.DiscountAmount;
            merchant.PurchaseTotalAmount += transaction.TotalAmount;
            merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
            merchant.PurchaseTotalMortgageAmount += transaction.TotalCageMortgageAmount;
            merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
            merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            context.Merchants.Update(merchant);
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
                    fruit.NumberOfKilogramsSold -= mtf.WeightInKilograms;
                    fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
                    fruit.PriceOfKilogramsSold -= mtf.PriceOfKiloGram * mtf.WeightInKilograms;
                    fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
                    fruit.RemainingCages += mtf.NumberOfCages;
                    fruit.SoldCages -= mtf.NumberOfCages;
                    if (fruit.RemainingCages == 0 && fruit.SoldCages == fruit.TotalCages)
                        fruit.Status = FruitStatus.NotAvailabe;
                    else
                        fruit.Status = FruitStatus.InStock;
                    if (fruit.IsCageHasMortgage)
                    {
                        transaction.TotalCageMortgageAmount -= fruit.CageMortgageValue * mtf.NumberOfCages;
                    }
                    context.Fruits.Update(fruit);
                }
            }
            var merchant = await context.Merchants
                .Include(c => c.Transactions)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == transaction.MerchantId);

            if (merchant is not null)
            {
                merchant.PurchasePrice -= transaction.Price;
                merchant.PurchaseTotalDiscountAmount -= transaction.DiscountAmount;
                merchant.PurchaseTotalAmount -= transaction.TotalAmount;
                merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
                merchant.PurchaseTotalMortgageAmount -= transaction.TotalCageMortgageAmount;
                merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
                merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
                merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
                context.Merchants.Update(merchant);
            }

            transaction.IsDeleted = true;
            context.Update(transaction);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
