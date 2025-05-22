using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.CashTransaction;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class CashTransactionService(ShaderContext context, IFruitService fruitService) : ICashTransactionService
    {
        public async Task<PagedResponse<RCashTDto>> GetCashTransactionsByDateAsync(DateOnly date, int pageNumber, int pageSize)
        {
            var transactions = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Where(c => DateOnly.FromDateTime(c.Date) == date && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return transactions.MapToRCashTDto().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RCashTDto>> GetAllCashTransactionsAsync(int pageNumber, int pageSize)
        {
            var transactions = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return transactions.MapToRCashTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        
        public async Task<PagedResponse<RCashTDto>> GetCashTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var transactions = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Where(c => !c.IsDeleted)
                .Where(c => DateOnly.FromDateTime(c.Date) >= startDate && DateOnly.FromDateTime(c.Date) <= endDate)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return transactions.MapToRCashTDto().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<RCashTDto> GetCashTransactionByIdAsync(int id)
        {
            var transaction = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) ??
                throw new Exception($"The transaction with id: ({id}) does not exist!");

            return transaction.MapToRCashTDto();
        }

        public async Task<RCashTDto> AddCashTransactionAsync(WCashTDto ctDto)
        {
            if (ctDto.CashTransactionFruits.All(c => c.FruitId == 0))
                throw new Exception("You must select a fruit");
            foreach (var ctf in ctDto.CashTransactionFruits)
            {
                if (ctDto.CashTransactionFruits.Where(c => c.FruitId == ctf.FruitId).Count() > 1)
                    throw new Exception("You can't add the same fruit multible times in the same transaction!");

                var fruit = await context.Fruits
                    .FirstOrDefaultAsync(f => f.Id == ctf.FruitId && !f.IsDeleted) ??
                    throw new Exception($"This fruit with id: ({ctf.FruitId}) dosen't exist");

                fruitService.UpdateTookFruitInCashTransaction(fruit, ctf);
            }
            var transaction = ctDto.MapToCashTransaction();
            await context.CashTransactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return transaction.MapToRCashTDto();
        }

        public async Task<RCashTDto> UpdateCashTransactionAsync(int id, WCashTDto ctDto)
        {
            var transaction = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) ?? 
                throw new Exception($"This transaction with id: ({id}) dosen't exist");

            foreach (var ctfDto in ctDto.CashTransactionFruits)
            {
                foreach (var ctf in transaction.CashTransactionFruits)
                {
                    bool addedFruitIsExist = ctDto.CashTransactionFruits.Any(c => c.FruitId == ctf.FruitId);
                    if (addedFruitIsExist)
                    {
                        var fruitEntity = await context.Fruits
                            .FirstOrDefaultAsync(f => f.Id == ctf.FruitId && !f.IsDeleted);

                        if (fruitEntity is not null)
                        {
                            fruitService.UpdateReturnedFruitInCashTransaction(fruitEntity, ctf);
                            fruitService.UpdateTookFruitInCashTransaction(fruitEntity, ctfDto);
                        }
                    }
                    else
                    {
                        var removedFruit = await context.Fruits
                            .FirstOrDefaultAsync(f => f.Id == ctf.FruitId && !f.IsDeleted);

                        if (removedFruit is not null)
                            fruitService.UpdateReturnedFruitInCashTransaction (removedFruit, ctf);

                        if (ctDto.CashTransactionFruits.Where(c => c.FruitId == ctfDto.FruitId).Count() > 1)
                            throw new Exception("You can't add a fruit multible times in the same transaction!");

                        var newFruit = await context.Fruits
                            .FirstOrDefaultAsync(f => f.Id == ctfDto.FruitId && !f.IsDeleted) ??
                            throw new Exception($"This fruit with id: ({ctfDto.FruitId}) dosen't exist");

                        fruitService.UpdateTookFruitInCashTransaction(newFruit, ctfDto);
                    }
                }
            }
            transaction = ctDto.MapToCashTransaction(transaction);
            context.CashTransactions.Update(transaction);
            await context.SaveChangesAsync();
            return transaction.MapToRCashTDto();
        }

        public async Task<bool> DeleteCashTransactionAsync(int id)
        {
            var transaction = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) ?? 
                throw new Exception($"This fruit with id: ({id}) dosen't exist");

            foreach (var ctf in transaction.CashTransactionFruits)
            {
                var fruit = await context.Fruits
                    .FirstOrDefaultAsync(f => f.Id == ctf.FruitId && !f.IsDeleted);

                if (fruit is not null)
                   fruitService.UpdateReturnedFruitInCashTransaction(fruit, ctf);
            }
            transaction.IsDeleted = true;
            context.CashTransactions.Update(transaction);
            return await context.SaveChangesAsync() > 0;
        }
        // todo : a method to get the sum of the cash transactions
    }
}
