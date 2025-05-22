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
    public class ClientTransactionService(ShaderContext context, IClientService clientService, IFruitService fruitService) : IClientTransactionService
    {
        public async Task<PagedResponse<RClientTDto>> GetTransactionsByClientIdAsync(int clientId, int pageNumber, int pageSize)
        {
            var client = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == clientId && !c.IsDeleted) ??
                throw new Exception($"The client with Id: ({clientId}) doesn't exist!!");

            var transactions = await context.ClientTransactions
                .Include(c => c.Client)
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Where(c => c.ClientId == clientId && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return transactions.MapToRClientTDto().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RClientTDto>> GetTransactionsByDateAsync(DateOnly date, int pageNumber, int pageSize)
        {
            var transactions = await context.ClientTransactions
                .Include(c => c.Client)
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Where(c => DateOnly.FromDateTime(c.Date) == date && !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return transactions.MapToRClientTDto().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RClientTDto>> GetAllTransactionsAsync(int pageNumber, int pageSize)

        {
            var transactions = await context.ClientTransactions
                .Include(c => c.Client)
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return transactions.MapToRClientTDto().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RClientTDto>> GetTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var transactions = await context.ClientTransactions
                .Include(c => c.Client)
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(c => c.Fruit)
                .Where(c => !c.IsDeleted)
                .Where(c => DateOnly.FromDateTime(c.Date) >= startDate && DateOnly.FromDateTime(c.Date) <= endDate)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            return transactions.MapToRClientTDto().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<RClientTDetailsDto> GetTransactionByIdAsync(int id)
        {
            var transaction = await context.ClientTransactions
                .Include(c => c.Client)
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) ?? 
                throw new Exception("This client transacttion dosen't exist");

            return transaction.MapToRClientTDetailsDto();
        }

        public async Task<RClientTDetailsDto> AddTransactionAsync(WClientTDto ctDto)
        {
            var client = await context.Clients
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == ctDto.ClientId && !c.IsDeleted) ?? 
                throw new Exception($"This client with Id({ctDto.ClientId}) dosen't exist!!");

            if (ctDto.ClientTransactionFruits.All(c => c.FruitId == 0)) throw new Exception("You must select a fruit!!");

            var transaction = ctDto.MapToClientTransaction();

            foreach (var ctf in ctDto.ClientTransactionFruits)
            {
                if (ctDto.ClientTransactionFruits.Where(c => c.FruitId == ctf.FruitId).Count() > 1)
                    throw new Exception($"You can't add a fruit multible times in the same transaction!");

                var fruit = await context.Fruits
                    .FirstOrDefaultAsync(f => f.Id == ctf.FruitId && !f.IsDeleted) ??
                    throw new Exception("This fruit dosen't exist");

                fruitService.UpdateTookFruitInClientTransaction(fruit, ctf);

                if (fruit.IsCageHasMortgage)
                    transaction.TotalCageMortgageAmount += fruit.CageMortgageValue * ctf.NumberOfCages;
            }
            clientService.UpdateClientWithIncreaseInTransaction(client, transaction); // update client data according to the transaction.
            await context.ClientTransactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return transaction.MapToRClientTDetailsDto();
        }

        public async Task<RClientTDetailsDto> UpdateTransactionAsync(int id, WClientTDto ctDto)
        {
            var client = await context.Clients
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == ctDto.ClientId && !c.IsDeleted) ?? 
                throw new Exception("This client dosen't exist");

            var transaction = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .ThenInclude(ctf => ctf.Fruit)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) ?? 
                throw new Exception("This transaction dosen't exist");

            var oldClientId = transaction.ClientId;
            if (oldClientId != ctDto.ClientId)
            {
                var removedClient = await context.Clients
                    .Include(c => c.Transactions)
                    .FirstOrDefaultAsync(c => c.Id == oldClientId && !c.IsDeleted);

                if (removedClient is not null)
                    clientService.UpdateClientWithDecreaseInTransaction(removedClient, transaction);
            }
            else
            {
                clientService.UpdateClientWithDecreaseInTransaction(client, transaction);
            }
            foreach (var ctfDto in ctDto.ClientTransactionFruits)
            {
                foreach (var ctf in transaction.ClientTransactionFruits)
                {
                    bool addedFruitIsExist = ctDto.ClientTransactionFruits.Any(c => c.FruitId == ctf.FruitId);
                    if (addedFruitIsExist)
                    {
                        var fruit = await context.Fruits
                            .FirstOrDefaultAsync(f => f.Id == ctf.FruitId && !f.IsDeleted);

                        if (fruit is not null)
                        {
                            fruitService.UpdateReturnedFruitInClientTransaction(fruit, ctf);
                            if (fruit.IsCageHasMortgage)
                                transaction.TotalCageMortgageAmount -= fruit.CageMortgageValue * ctf.NumberOfCages;

                            fruitService.UpdateTookFruitInClientTransaction(fruit, ctfDto);
                            if (fruit.IsCageHasMortgage)
                                transaction.TotalCageMortgageAmount += fruit.CageMortgageValue * ctf.NumberOfCages;
                        }
                    }
                    else
                    {
                        var removedFruit = await context.Fruits
                            .FirstOrDefaultAsync(f => f.Id == ctf.FruitId && !f.IsDeleted);

                        if (removedFruit is not null)
                        {
                            fruitService.UpdateReturnedFruitInClientTransaction(removedFruit, ctf);
                            if (removedFruit.IsCageHasMortgage)
                                transaction.TotalCageMortgageAmount -= removedFruit.CageMortgageValue * ctf.NumberOfCages;
                        }

                        if (ctDto.ClientTransactionFruits.Where(c => c.FruitId == ctfDto.FruitId).Count() > 1)
                            throw new Exception("You can't add the same fruit multible times in the same transaction!");

                        var newFruit = await context.Fruits
                            .FirstOrDefaultAsync(f => f.Id == ctfDto.FruitId && !f.IsDeleted) ??
                            throw new Exception($"The fruit with id :({ctfDto.FruitId}) dosen't exist!");

                        fruitService.UpdateTookFruitInClientTransaction(newFruit, ctfDto);
                        if (newFruit.IsCageHasMortgage)
                            transaction.TotalCageMortgageAmount += newFruit.CageMortgageValue * ctfDto.NumberOfCages;
                    }
                }
            }
            ctDto.MapToClientTransaction(transaction);
            context.ClientTransactions.Update(transaction);
            clientService.UpdateClientWithIncreaseInTransaction(client, transaction);
            await context.SaveChangesAsync();
            return transaction.MapToRClientTDetailsDto();
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await context.ClientTransactions
                .Include(c => c.ClientTransactionFruits)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) ?? throw new Exception("This transaction dosen't exist");


            foreach (var ctf in transaction.ClientTransactionFruits)
            {
                var fruit = await context.Fruits
                    .FirstOrDefaultAsync(f => f.Id == ctf.FruitId && !f.IsDeleted);

                if(fruit is not null)
                    fruitService.UpdateReturnedFruitInClientTransaction(fruit, ctf);
            }
            transaction.IsDeleted = true;
            context.ClientTransactions.Update(transaction);

            var client = await context.Clients
                .Include(c=> c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == transaction.ClientId && !c.IsDeleted);

            if (client is not null)
                clientService.UpdateClientWithDecreaseInTransaction(client, transaction);

            return await context.SaveChangesAsync() > 0;
        }
        
    }
}
