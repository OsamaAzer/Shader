using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Mapping;
using Shader.Services.Abstraction;
using System.Linq;

namespace Shader.Services.Implementation
{
    public class ClientTransactionService(ShaderContext context) : IClientTransactionService
    {
        public async Task<IEnumerable<ClientTransaction>> GetAllClientTransactionsAsync()
        {
            var transactions = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .OrderByDescending(c => c.Date)
                .OrderByDescending(c => c.Time)
                .OrderDescending()
                .ToListAsync();
            return transactions;
        }
        public async Task<ClientTransaction> GetClientTransactionByIdAsync(int id)
        {
            var transaction = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            if (transaction is null) return null;
            return transaction;
        }
        public async Task<IEnumerable<ClientTransaction>> GetClientTransactionsByDateAndTimeRangeAsync
            (DateOnly? startDate, DateOnly? endDate, TimeOnly? startTime, TimeOnly? endTime)
        {
            var transactions = context.ClientTransactions.Include(c => c.ClientTransactionFruits).AsQueryable();

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
        public async Task<ClientTransaction> AddClientTransactionAsync(WClientTransactionDTO clientTransactionDTO)
        {
            if (clientTransactionDTO.ClientTransactionFruits.All(c => c.FruitId == 0))
                throw new Exception("You must select a fruit");
            foreach (var ctf in clientTransactionDTO.ClientTransactionFruits)
            {
                if (clientTransactionDTO.ClientTransactionFruits.Where(c => c.FruitId == ctf.FruitId).Count() > 1)
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
            var transaction = clientTransactionDTO.MapTo<ClientTransaction>();
            transaction.Date = DateOnly.FromDateTime(DateTime.Now);
            transaction.Time = TimeOnly.FromDateTime(DateTime.Now);
            transaction.TotalAmount = clientTransactionDTO.ClientTransactionFruits
                .Select(c => c.PriceOfKiloGram * c.WeightInKilograms).Sum();
            //transaction.TotalCageMortgageAmount = transaction.ClientTransactionFruits.Select(t => t.NumberOfCages * t.Fruit.CageMortgageValue).Sum();
            transaction.RemainingAmount = transaction.TotalAmount - clientTransactionDTO.AmountPaid;
            transaction.RemainingMortgageAmount = transaction.TotalCageMortgageAmount - clientTransactionDTO.TotalCageMortgageAmountPaid;
            await context.ClientTransactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return transaction;
        }
        public async Task<ClientTransaction> UpdateClientTransactionAsync(int id, WClientTransactionDTO clientTransactionDTO)
        {
            var transaction = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (transaction is null) return null;
            foreach (var ctf in transaction.ClientTransactionFruits)
            {
                bool flag = clientTransactionDTO.ClientTransactionFruits.Any(c => c.FruitId == ctf.FruitId);
                if (!flag)
                {
                    var fruitEntity = await context.Fruits.Where(f => f.Id == ctf.FruitId).FirstOrDefaultAsync();
                    if (fruitEntity is null) throw new Exception("This fruit dosen't exist");
                    fruitEntity.NumberOfKilogramsSold -= ctf.WeightInKilograms;
                    fruitEntity.NumberOfKilogramsSold = Math.Round(fruitEntity.NumberOfKilogramsSold, 2);
                    fruitEntity.PriceOfKilogramsSold -= ctf.PriceOfKiloGram * ctf.WeightInKilograms;
                    fruitEntity.PriceOfKilogramsSold = Math.Round(fruitEntity.PriceOfKilogramsSold, 2);
                    fruitEntity.RemainingCages += ctf.NumberOfCages;
                    fruitEntity.SoldCages -= ctf.NumberOfCages;
                    if (fruitEntity.RemainingCages == 0)
                        fruitEntity.Status = FruitStatus.NotAvailabe;
                    context.Fruits.Update(fruitEntity);
                }
            }
            foreach (var ctfDto in clientTransactionDTO.ClientTransactionFruits)
            {
                if (clientTransactionDTO.ClientTransactionFruits.Where(c => c.FruitId == ctfDto.FruitId).Count() > 1)
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
            clientTransactionDTO.MapTo(transaction);
            transaction.TotalAmount = clientTransactionDTO.ClientTransactionFruits
               .Select(c => c.PriceOfKiloGram * c.WeightInKilograms).Sum();
            //transaction.TotalCageMortgageAmount = transaction.ClientTransactionFruits.Select(t => t.NumberOfCages * t.Fruit.CageMortgageValue).Sum();
            transaction.RemainingAmount = transaction.TotalAmount - clientTransactionDTO.AmountPaid;
            transaction.RemainingMortgageAmount = transaction.TotalCageMortgageAmount - clientTransactionDTO.TotalCageMortgageAmountPaid;
            context.ClientTransactions.Update(transaction);
            await context.SaveChangesAsync();
            return transaction;
        }
        public async Task<bool> DeleteClientTransactionAsync(int id)
        {
            var transaction = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            if (transaction is null) return false;
            foreach (var ctf in transaction.ClientTransactionFruits)
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
            context.ClientTransactions.Remove(transaction);
            return await context.SaveChangesAsync() > 0;
        }
        
    }
}
