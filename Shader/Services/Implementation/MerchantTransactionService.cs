using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.ShaderTransaction;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class MerchantTransactionService(ShaderContext context) : IMerchantTransactionService
    {
        public async Task<IEnumerable<RMerchantTDto>> GetAllTransactionsAsync()
        {
            var transactions = await context.MerchantTransactions
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Merchant)
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .OrderDescending()
                .ToListAsync();
            return transactions.MapToRMerchantTDto();

        }
        public async Task<IEnumerable<RMerchantTDto>> GetAllTransactionsByDateAsync(DateOnly date)
        {
            var transactions = await context.MerchantTransactions
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Merchant)
                .Where(c => DateOnly.FromDateTime(c.Date) == date && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
            return transactions.MapToRMerchantTDto();
        }
        public async Task<IEnumerable<RMerchantTDto>> GetAllTransactionsByDateRangeAsync(DateOnly startDate, DateOnly endDate)
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
                .OrderByDescending(c => c.Date.Hour)
                .ToListAsync();

            return transactions.MapToRMerchantTDto();
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
        public async Task<IEnumerable<RMerchantTDto>> GetTransactionsByShaderIdAsync(int shaderId)
        {
            var merchant = await context.Merchants
            .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == shaderId) ??
                throw new Exception($"The merchant with Id: ({shaderId}) doesn't exist!!");

            var transactions = await context.MerchantTransactions
                .Include(c => c.MerchantTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Merchant)
                .Where(c => c.MerchantId == shaderId && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .OrderDescending()
                .ToListAsync();
            return transactions.MapToRMerchantTDto();
        }
        public async Task<RMerchantTDetailsDto> CreateTransactionAsync(WMerchantTDto mtDto)
        {
            var merchant = await context.Merchants
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == mtDto.ShaderId) ??
                throw new Exception($"This Shader with Id({mtDto.ShaderId}) dosen't exist!!");

            if (mtDto.MerchantTransactionFruits.All(c => c.FruitId == 0)) throw new Exception("You must select a fruit!!");

            var transaction = mtDto.MapToMerchantTransaction();

            foreach (var mtf in mtDto.MerchantTransactionFruits)
            {
                if (mtDto.MerchantTransactionFruits.Where(c => c.FruitId == mtf.FruitId).Count() > 1)
                    throw new Exception("You can't add the same fruit multible times in the same transaction!");

                var fruit = await context.Fruits
                    .Where(f => !f.IsDeleted)
                    .FirstOrDefaultAsync(f => f.Id == mtf.FruitId) ?? throw new Exception("This fruit dosen't exist");

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
                    //client.TotalMortgageAmount += fruit.CageMortgageValue * ctf.NumberOfCages;
                }
                context.Fruits.Update(fruit);
            }

            //transaction.RemainingMortgageAmount = transaction.TotalCageMortgageAmount - transaction.TotalCageMortgageAmountPaid;
            await context.MerchantTransactions.AddAsync(transaction);
            //merchant = await MerchantService.UpdateMerchantAggregatesAsync(merchant.Id); // update client data according to the transaction.
            context.Merchants.Update(merchant);
            await context.SaveChangesAsync();
            return transaction.MapToRMerchantTDetailsDto();
        }
        public async Task<RMerchantTDetailsDto> UpdateTransactionAsync(int id, WMerchantTDto transactionDto)
        {
            return new RMerchantTDetailsDto();
        }
        public async Task<bool> DeleteTransactionAsync(int id)
        {
            return false;
        }
    }
}
