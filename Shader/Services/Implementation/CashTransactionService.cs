using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Mapping;
using Shader.Services.Abstraction;
using static System.TimeZoneInfo;

namespace Shader.Services.Implementation
{
    public class CashTransactionService(ShaderContext context) : ICashTransactionService
    {
        

        public async Task<IEnumerable<CashTransaction>> GetAllCashTransactionsAsync()
        {
            var transactions = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .OrderByDescending(c => c.Date)
                .OrderByDescending(c => c.Time)
                .OrderDescending()
                .ToListAsync();
            return transactions;
        }
        public async Task<CashTransaction> GetCashTransactionByIdAsync(int id)
        {
            var transaction = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            if (transaction is null) return null;
            return transaction;
        }
        public async Task<IEnumerable<CashTransaction>> GetCashTransactionsByDateAndTimeRangeAsync
            (DateOnly? startDate, DateOnly? endDate, TimeOnly? startTime, TimeOnly? endTime)
        {
            var transactions = context.CashTransactions.Include(c => c.CashTransactionFruits).AsQueryable();

            if (startDate is not null && endDate is not null)
                transactions = transactions.Where(e => e.Date >= startDate && e.Date <= endDate);

            if (startTime is not null && endTime is not null)
                transactions = transactions.Where(e => e.Date == DateOnly.FromDateTime(DateTime.Now))
                    .Where(e => e.Time >= startTime && e.Time <= endTime);

            await transactions.OrderByDescending(c => c.Date)
                .OrderByDescending(c => c.Time)
                .OrderDescending()
                .ToListAsync();

            return transactions;
        }
        public async Task<CashTransaction> AddCashTransactionAsync(WCashTransactionDTO cashTransactionDTO)
        {
            if (cashTransactionDTO.CashTransactionFruits.All(c => c.FruitId == 0))
                throw new Exception("You must select a fruit");
            foreach (var ctf in cashTransactionDTO.CashTransactionFruits)
            {
                if (cashTransactionDTO.CashTransactionFruits.Where(c => c.FruitId == ctf.FruitId).Count() > 1)
                    throw new Exception("You can't add the same fruit multible times in the same transaction!");
                var fruit = await context.Fruits.Where(f => f.Id == ctf.FruitId).FirstOrDefaultAsync();
                if (fruit is null) throw new Exception("This fruit dosen't exist");
                fruit.NumberOfKilogramsSold += ctf.WeightInKilograms;
                fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
                fruit.PriceOfKilogramsSold += ctf.PriceOfKiloGram * ctf.WeightInKilograms;
                fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
                fruit.RemainingCages -= ctf.NumberOfCages;
                fruit.SoldCages += ctf.NumberOfCages;
                if (fruit.RemainingCages == 0)
                    fruit.Status = FruitStatus.NotAvailabe;
                context.Fruits.Update(fruit);
            }
            var transaction = cashTransactionDTO.MapTo<CashTransaction>();
            transaction.Date = DateOnly.FromDateTime(DateTime.Now);
            transaction.Time = TimeOnly.FromDateTime(DateTime.Now);
            transaction.TotalAmount = cashTransactionDTO.CashTransactionFruits
                    .Select(c => c.PriceOfKiloGram * c.WeightInKilograms).Sum();
            await context.CashTransactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return transaction;
        }
        public async Task<CashTransaction> UpdateCashTransactionAsync(int id, WCashTransactionDTO cashTransactionDTO)
        {
            var transaction = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            if (transaction is null) return null;
            foreach (var ctf in transaction.CashTransactionFruits)
            {
                bool flag = cashTransactionDTO.CashTransactionFruits.Any(c => c.FruitId == ctf.FruitId);
                if (!flag)
                {
                    var fruitEntity = await context.Fruits.Where(f => f.Id == ctf.FruitId).FirstOrDefaultAsync();
                    if (fruitEntity is null) throw new Exception("This fruit dosen't exist");
                    fruitEntity.NumberOfKilogramsSold -= ctf.WeightInKilograms;
                    fruitEntity.PriceOfKilogramsSold -= ctf.PriceOfKiloGram * ctf.WeightInKilograms;
                    fruitEntity.PriceOfKilogramsSold = Math.Round(fruitEntity.PriceOfKilogramsSold, 2);
                    fruitEntity.RemainingCages += ctf.NumberOfCages;
                    fruitEntity.SoldCages -= ctf.NumberOfCages;
                    if (fruitEntity.RemainingCages == 0)
                        fruitEntity.Status = FruitStatus.NotAvailabe;
                    context.Fruits.Update(fruitEntity);
                }
            }
            foreach (var ctfDto in cashTransactionDTO.CashTransactionFruits)
            {
                if (cashTransactionDTO.CashTransactionFruits.Where(c => c.FruitId == ctfDto.FruitId).Count() > 1)
                    throw new Exception("You can't add the same fruit multible times in the same transaction!");
                var fruit = await context.Fruits.Where(f => f.Id == ctfDto.FruitId).FirstOrDefaultAsync();
                if (fruit is null) throw new Exception("This fruit dosen't exist");
                fruit.NumberOfKilogramsSold += ctfDto.WeightInKilograms;
                fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
                fruit.PriceOfKilogramsSold += ctfDto.PriceOfKiloGram * ctfDto.WeightInKilograms;
                fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
                fruit.RemainingCages -= ctfDto.NumberOfCages;
                fruit.SoldCages += ctfDto.NumberOfCages;
                if (fruit.RemainingCages == 0)
                    fruit.Status = FruitStatus.NotAvailabe;
                context.Fruits.Update(fruit);
            }
            transaction.Description = cashTransactionDTO.Description;
            transaction.TotalAmount = cashTransactionDTO.CashTransactionFruits
               .Select(c => c.PriceOfKiloGram * c.WeightInKilograms).Sum();
            transaction.CashTransactionFruits = cashTransactionDTO.CashTransactionFruits
               .Select(c => c.ToEntity<CashTransactionFruitDTO, CashTransactionFruit>()).ToList();
            context.CashTransactions.Update(transaction);
            await context.SaveChangesAsync();
            return transaction;
        }
        public async Task<bool> DeleteCashTransactionAsync(int id)
        {
            var transaction = await context.CashTransactions
                .Include(c => c.CashTransactionFruits)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            if (transaction is null) return false;
            foreach (var ctf in transaction.CashTransactionFruits)
            {
                var fruit = await context.Fruits.Where(f => f.Id == ctf.FruitId).FirstOrDefaultAsync();
                if (fruit is null) throw new Exception("This fruit dosen't exist");
                fruit.NumberOfKilogramsSold -= ctf.WeightInKilograms;
                fruit.NumberOfKilogramsSold = Math.Round(fruit.NumberOfKilogramsSold, 2);
                fruit.PriceOfKilogramsSold -= ctf.PriceOfKiloGram * ctf.WeightInKilograms;
                fruit.PriceOfKilogramsSold = Math.Round(fruit.PriceOfKilogramsSold, 2);
                fruit.RemainingCages += ctf.NumberOfCages;
                fruit.SoldCages -= ctf.NumberOfCages;
                if (fruit.RemainingCages == 0)
                    fruit.Status = FruitStatus.NotAvailabe;
                context.Fruits.Update(fruit);
            }
            context.CashTransactions.Remove(transaction);
            return await context.SaveChangesAsync() > 0;
        }
        
    }
}
