using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Shader.Data;
using Shader.Data.Dtos.CashTransaction;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;
using static System.TimeZoneInfo;

namespace Shader.Services.Implementation
{
    public class CashTransactionService(ShaderContext context) : ICashTransactionService
    {
        public async Task<PagedResponse<RCashTDto>> GetCashTransactionsByDateAsync(DateOnly date, int pageNumber, int pageSize)
        {
            var transactions = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Where(c => DateOnly.FromDateTime(c.Date) == date && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .OrderByDescending(c => c.Date.Hour)
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
                .OrderByDescending(c => c.Date.Hour)
                .OrderDescending()
                .ToListAsync();
            return transactions.MapToRCashTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<RCashTDto> GetCashTransactionByIdAsync(int id)
        {
            var transaction = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id) ?? 
                throw new Exception($"The transaction with id: ({id}) does not exist!");

            return transaction.MapToRCashTDto();
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
                .OrderByDescending(c => c.Date.Hour)
                .ToListAsync();

            return transactions.MapToRCashTDto().CreatePagedResponse(pageNumber, pageSize);
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
                    .Where(f => !f.IsDeleted)
                    .FirstOrDefaultAsync(f => f.Id == ctf.FruitId) ??
                    throw new Exception($"This fruit with id: ({ctf.FruitId}) dosen't exist");

                if ((fruit.RemainingCages == 0) && (fruit.SoldCages == fruit.TotalCages) && (fruit.Status == FruitStatus.NotAvailabe))
                    throw new Exception($"This fruit with id: ({ctf.FruitId}) is not available");

                fruit.NumberOfKilogramsSold += ctf.WeightInKilograms;
                fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
                fruit.PriceOfKilogramsSold += ctf.PriceOfKiloGram * ctf.WeightInKilograms;
                fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
                fruit.RemainingCages -= ctf.NumberOfCages;
                fruit.SoldCages += ctf.NumberOfCages;

                if (fruit.RemainingCages == 0 && fruit.SoldCages == fruit.TotalCages)
                    fruit.Status = FruitStatus.NotAvailabe;
                else
                    fruit.Status = FruitStatus.InStock;

                context.Fruits.Update(fruit);
            }
            var transaction = ctDto.MapToCashTransaction();
            //transaction.Date = DateOnly.FromDateTime(DateTime.Now);
            //transaction.Time = TimeOnly.FromDateTime(DateTime.Now);
            //transaction.Price = ctDto.CashTransactionFruits
            //        .Select(c => c.PriceOfKiloGram * c.WeightInKilograms).Sum();
            await context.CashTransactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return transaction.MapToRCashTDto();
        }
        public async Task<RCashTDto> UpdateCashTransactionAsync(int id, WCashTDto ctDto)
        {
            var transaction = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync() ?? throw new Exception($"This transaction with id: ({id}) dosen't exist");

            foreach (var ctf in transaction.CashTransactionFruits)
            {
                bool flag = ctDto.CashTransactionFruits.Any(c => c.FruitId == ctf.FruitId);
                if (!flag)
                {
                    var fruitEntity = await context.Fruits.Where(f => f.Id == ctf.FruitId).FirstOrDefaultAsync();
                    if (fruitEntity is null) throw new Exception($"This fruit with id: ({ctf.FruitId}) dosen't exist");
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
                }
            }
            foreach (var ctfDto in ctDto.CashTransactionFruits)
            {
                if (ctDto.CashTransactionFruits.Where(c => c.FruitId == ctfDto.FruitId).Count() > 1)
                    throw new Exception("You can't add the same fruit multible times in the same transaction!");
                var fruit = await context.Fruits.Where(f => f.Id == ctfDto.FruitId).FirstOrDefaultAsync();
                if (fruit is null) throw new Exception($"This fruit with id: ({ctfDto.FruitId}) dosen't exist");
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
            }
            //transaction.Description = ctDto.Description;
            //transaction.Price = ctDto.CashTransactionFruits
            //   .Select(c => c.PriceOfKiloGram * c.WeightInKilograms).Sum();
            //transaction.CashTransactionFruits = ctDto.CashTransactionFruits
            //   .Select(c => c.ToEntity<WCashTFruitDto, CashTransactionFruit>()).ToList();
            transaction = ctDto.MapToCashTransaction(transaction);
            context.CashTransactions.Update(transaction);
            await context.SaveChangesAsync();
            return transaction.MapToRCashTDto();
        }
        public async Task<bool> DeleteCashTransactionAsync(int id)
        {
            var transaction = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .Where(c => c.Id == id && !c.IsDeleted)
                .FirstOrDefaultAsync();
            if (transaction is null) throw new Exception($"This fruit with id: ({id}) dosen't exist");
            foreach (var ctf in transaction.CashTransactionFruits)
            {
                var fruit = await context.Fruits.Where(f => f.Id == ctf.FruitId).FirstOrDefaultAsync();
                if (fruit is null) throw new Exception($"This fruit with id: ({ctf.FruitId}) dosen't exist");
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
            }
            transaction.IsDeleted = true;
            context.CashTransactions.Update(transaction);
            return await context.SaveChangesAsync() > 0;
        }
        // todo : a method to get the sum of the cash transactions
    }
}
