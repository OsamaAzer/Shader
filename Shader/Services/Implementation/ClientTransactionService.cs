using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Shader.Data;
using Shader.Data.Dtos.Client;
using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Shader.Services.Implementation
{
    public class ClientTransactionService(ShaderContext context, IClientService clientService) : IClientTransactionService
    {
        public async Task<PagedResponse<RClientTDto>> GetUnPaidClientTransactionsByClientIdAsync(int clientId, int pageNumber, int pageSize)
        {
            var client = await context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == clientId) ??
                throw new Exception($"The client with Id: ({clientId}) doesn't exist!!");

            var transactions = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Client)
                .Where(c => c.ClientId == clientId && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
            return transactions.MapToRClientTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RClientTDto>> GetClientTransactionsByClientIdAsync(int clientId, int pageNumber, int pageSize)
        {
            var client = await context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == clientId) ??
                throw new Exception($"The client with Id: ({clientId}) doesn't exist!!");

            var transactions = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Client)
                .Where(c => c.ClientId == clientId && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
            return transactions.MapToRClientTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RClientTDto>> GetClientTransactionsByDateAsync(DateOnly date, int pageNumber, int pageSize)
        {
            var transactions = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Client)
                .Where(c => DateOnly.FromDateTime(c.Date) == date && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
            return transactions.MapToRClientTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RClientTDto>> GetAllClientTransactionsAsync(int pageNumber, int pageSize)
        {
            var transactions = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Client)
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();
            return transactions.MapToRClientTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RClientTDto>> GetClientTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var transactions = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Include(c => c.Client)
                .Where(c => !c.IsDeleted)
                .Where(c => DateOnly.FromDateTime(c.Date) >= startDate && DateOnly.FromDateTime(c.Date) <= endDate)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return transactions.MapToRClientTDto().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<RClientTDetailsDto> GetClientTransactionByIdAsync(int id)
        {
            var transaction = await context.ClientTransactions
                .Where(c => !c.IsDeleted)
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("This client transacttion dosen't exist");
            return transaction.MapToRClientTDetailsDto();
        }
        public async Task<RClientTDetailsDto> AddClientTransactionAsync(WClientTDto ctDto)
        {
            var client = await context.Clients
                .Include(c => c.Transactions)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == ctDto.ClientId) ?? 
                throw new Exception($"This client with Id({ctDto.ClientId}) dosen't exist!!");

            if (ctDto.ClientTransactionFruits.All(c => c.FruitId == 0)) throw new Exception("You must select a fruit!!");

            var transaction = ctDto.MapToClientTransaction();

            foreach (var ctf in ctDto.ClientTransactionFruits)
            {
                if (ctDto.ClientTransactionFruits.Where(c => c.FruitId == ctf.FruitId).Count() > 1)
                    throw new Exception("You can't add the same fruit multible times in the same transaction!");

                var fruit = await context.Fruits
                    .Where(f => !f.IsDeleted)
                    .FirstOrDefaultAsync(f => f.Id == ctf.FruitId) ?? throw new Exception("This fruit dosen't exist");

                if (fruit is not null)
                {
                    if (fruit.RemainingCages == 0 && fruit.Status == FruitStatus.NotAvailabe)
                        throw new Exception("The number of cages is not enough");
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

                    if (fruit.IsCageHasMortgage)
                    {
                        transaction.TotalCageMortgageAmount += fruit.CageMortgageValue * ctf.NumberOfCages;
                    }
                    context.Fruits.Update(fruit);
                }
            }

            await context.ClientTransactions.AddAsync(transaction);
            client = await clientService.UpdateClientAggregatesAsync(client); // update client data according to the transaction.
            context.Clients.Update(client);
            await context.SaveChangesAsync();
            return transaction.MapToRClientTDetailsDto();
        }
        public async Task<RClientTDetailsDto> UpdateClientTransactionAsync(int id, WClientTDto ctDto)
        {
            var client = await context.Clients
                .Include(c => c.Transactions)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == ctDto.ClientId) ?? throw new Exception("This client dosen't exist");

            var transaction = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("This transaction dosen't exist");

            foreach (var ctf in transaction.ClientTransactionFruits)
            {
                bool flag = ctDto.ClientTransactionFruits.Any(c => c.FruitId == ctf.FruitId);
                if (!flag)
                {
                    var removedFruit = await context.Fruits
                        .Where(f => f.Id == ctf.FruitId)
                        .Where(f => !f.IsDeleted)
                        .FirstOrDefaultAsync();
                    if (removedFruit is not null)
                    {
                        removedFruit.NumberOfKilogramsSold -= ctf.WeightInKilograms;
                        removedFruit.NumberOfKilogramsSold = Math.Round(removedFruit.NumberOfKilogramsSold, 2);
                        removedFruit.PriceOfKilogramsSold -= ctf.PriceOfKiloGram * ctf.WeightInKilograms;
                        removedFruit.PriceOfKilogramsSold = Math.Round(removedFruit.PriceOfKilogramsSold, 2);
                        removedFruit.RemainingCages += ctf.NumberOfCages;
                        removedFruit.SoldCages -= ctf.NumberOfCages;

                        if (removedFruit.RemainingCages == 0 && removedFruit.SoldCages == removedFruit.TotalCages)
                            removedFruit.Status = FruitStatus.NotAvailabe;
                        else
                            removedFruit.Status = FruitStatus.InStock;

                        if (removedFruit.IsCageHasMortgage)
                        {
                            transaction.TotalCageMortgageAmount -= removedFruit.CageMortgageValue * ctf.NumberOfCages;
                        }

                        context.Fruits.Update(removedFruit);
                    }
                }
                else
                {
                    foreach (var ctfDto in ctDto.ClientTransactionFruits)
                    {
                        if (ctDto.ClientTransactionFruits.Where(c => c.FruitId == ctfDto.FruitId).Count() > 1)
                            throw new Exception("You can't add the same fruit multible times in the same transaction!");

                        var fruit = await context.Fruits
                            .Where(f => !f.IsDeleted)
                            .FirstOrDefaultAsync(f => f.Id == ctfDto.FruitId);

                        if (fruit is not null)
                        {
                            if (fruit.RemainingCages == 0) throw new Exception("The number of cages is not enough");
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

                            if (fruit.IsCageHasMortgage)
                            {
                                transaction.TotalCageMortgageAmount -= fruit.CageMortgageValue * ctf.NumberOfCages;
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

            var oldClientId = transaction.ClientId;

            if (oldClientId == ctDto.ClientId)
            {
                client.Price -= transaction.Price;
                client.TotalAmount -= transaction.TotalAmount;
                client.TotalRemainingAmount = client.TotalAmount - client.AmountPaid;
                client.TotalDiscountAmount -= transaction.DiscountAmount;
                client.TotalMortgageAmount -= transaction.TotalCageMortgageAmount;
                client.TotalRemainingMortgageAmount = client.TotalMortgageAmount - client.TotalMortgageAmountPaid;
                //client = await clientService.UpdateClientAggregatesAsync(client.Id);
                //context.Clients.Update(client);
            }

            if (oldClientId != ctDto.ClientId)
            {
                var removedClient = await context.Clients
                    .Include(c => c.Transactions)
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == oldClientId);

                if (removedClient is not null)
                {
                    removedClient.Price -= transaction.Price;
                    removedClient.TotalAmount -= transaction.TotalAmount;
                    removedClient.TotalRemainingAmount = client.TotalAmount - client.AmountPaid;
                    removedClient.TotalDiscountAmount -= transaction.DiscountAmount;
                    removedClient.TotalMortgageAmount -= transaction.TotalCageMortgageAmount;
                    removedClient.TotalRemainingMortgageAmount = client.TotalMortgageAmount - client.TotalMortgageAmountPaid;
                    context.Clients.Update(removedClient);
                    //removedClient = await clientService.UpdateClientAggregatesAsync(removedClient.Id);
                    //context.Clients.Update(removedClient);
                }
            }


            transaction = ctDto.MapToClientTransaction(transaction);
            context.ClientTransactions.Update(transaction);

            client.Price += transaction.Price;
            client.TotalAmount += transaction.Price - transaction.DiscountAmount;
            client.TotalRemainingAmount = client.TotalAmount - client.AmountPaid;
            client.TotalDiscountAmount += transaction.DiscountAmount;
            client.TotalMortgageAmount += transaction.TotalCageMortgageAmount;
            client.TotalRemainingMortgageAmount = client.TotalMortgageAmount - client.TotalMortgageAmountPaid;
            context.Clients.Update(client);
            await context.SaveChangesAsync();
            return transaction.MapToRClientTDetailsDto();
        }
        public async Task<RClientTDetailsDto> UpdateClientTransactionWithPayments
            (int id, decimal paidAmount, decimal discountAmount, decimal cageMortgageAmountPaid)
        {

            var transaction = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .Where(c =>!c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("The transaction doesn't exist!");

            var client = context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefault(c => c.Id == transaction.ClientId) ?? throw new Exception("The client doesn't exist!");

            //if (transaction.TotalCageMortgageAmount == transaction.TotalCageMortgageAmountPaid && transaction.RemainingMortgageAmount == 0 && transaction.TotalCageMortgageAmountPaid > 0)
            //    throw new Exception("The cage mortgage amount was paid!");
            //if (transaction.AmountPaid == transaction.TotalAmount && transaction.RemainingAmount == 0 && transaction.AmountPaid > 0)
            //    throw new Exception("The transaction amount was paid!"); 
            //if (paidAmount + discountAmount > transaction.RemainingAmount)
            //    throw new Exception("The amount paid and discount amount can't be greater than the remaining amount!");
            //if (paidAmount > transaction.RemainingAmount)
            //    throw new Exception("The amount paid is greater than the remaining amount!");
            //if (discountAmount > transaction.RemainingAmount) 
            //    throw new Exception("The discount amount can't be greater than or equal remaining amount!");
            //if (cageMortgageAmountPaid > transaction.TotalCageMortgageAmount && cageMortgageAmountPaid > transaction.RemainingMortgageAmount)
            //    throw new Exception("The cage mortgage amount paid can't be greater than the remaining cage mortgage amount!");
            

            //transaction.AmountPaid += paidAmount;

            if (transaction.DiscountAmount >= 0 && discountAmount > 0)
                transaction.DiscountAmount = discountAmount;

            transaction.TotalAmount = transaction.Price - transaction.DiscountAmount;
            //transaction.RemainingAmount = transaction.TotalAmount - transaction.AmountPaid;
            //transaction.TotalCageMortgageAmountPaid += cageMortgageAmountPaid;
            //transaction.RemainingMortgageAmount = transaction.TotalCageMortgageAmount - transaction.TotalCageMortgageAmountPaid;

            context.ClientTransactions.Update(transaction);
            client = await clientService.UpdateClientAggregatesAsync(client);
            context.Clients.Update(client);
            await context.SaveChangesAsync();
            return transaction.MapToRClientTDetailsDto();
        }
        public async Task<bool> DeleteClientTransactionAsync(int id)
        {
            var transaction = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("This transaction dosen't exist");


            foreach (var ctf in transaction.ClientTransactionFruits)
            {
                var fruit = await context.Fruits
                    .Where(f => f.Id == ctf.FruitId)
                    .Where(f => !f.IsDeleted)
                    .FirstOrDefaultAsync();
                if(fruit is not null)
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
                }
            }
            transaction.IsDeleted = true;
            //client.Price -= transaction.Price;
            //client.TotalAmount -= transaction.TotalAmount;
            //client.AmountPaid -= transaction.AmountPaid;
            //client.TotalRemainingAmount = client.TotalAmount - client.AmountPaid;
            //client.TotalDiscountAmount -= transaction.DiscountAmount;
            //client.TotalMortgageAmount -= transaction.TotalCageMortgageAmount;
            //client.TotalMortgageAmountPaid -= transaction.TotalCageMortgageAmountPaid;
            //client.TotalRemainingMortgageAmount = client.TotalMortgageAmount - client.TotalMortgageAmountPaid;
            //context.Clients.Update(client);
            context.ClientTransactions.Update(transaction);
            var client = await context.Clients
                .Include(c=> c.Transactions)
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == transaction.ClientId);

            if (client is not null)
            {
                client = await clientService.UpdateClientAggregatesAsync(client);
                context.Update(client);
            }
            return await context.SaveChangesAsync() > 0;
        }
        
    }
}
