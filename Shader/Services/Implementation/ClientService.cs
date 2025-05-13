using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.Client;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class ClientService(ShaderContext context) : IClientService
    {
        public async Task<RClientDto> GetClientByIdAsync(int id)
        {
            var client = await context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id) ?? 
                throw new Exception($"Client with id:({id}) does not exist!");

            return client.Map<Client, RClientDto>();
        }
        public async Task<IEnumerable<RAllClientsDto>> GetAllClientsAsync()
        {
            var clientsDto =  context.Clients
                            .Where(c => !c.IsDeleted)
                            .OrderBy(c => c.Name);
            return await Task.FromResult(clientsDto.Map<Client, RAllClientsDto>());
        }
        public async Task<IEnumerable<RAllClientsDto>> GetAllClientsWithNameAsync(string name)
        {
            var clients = await context.Clients
                        .Where(c => !c.IsDeleted)
                        .Where(c =>c.Name.ToLower().Contains(name.ToLower()))
                        .OrderBy(c => c.Name)
                        .ToListAsync();

            return await Task.FromResult(clients.Map<Client, RAllClientsDto>());
        }
        public async Task<RClientDto> AddClientAsync(WClientDto dto)
        {
            var client = dto.Map<WClientDto, Client>();
            client.Status = Status.InActive;
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
            return client.Map<Client, RClientDto>();
        }
        public async Task<Client> UpdateClientAggregatesAsync(Client client)
        {
            //var client = await context.Clients
            //    .Include(c => c.Transactions.Where(t => !t.IsDeleted && t.ClientId == id))
            //    .Where(c => !c.IsDeleted)
            //    .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception($"The client with Id: ({id}) does not exist!");

            client.Price = client.Transactions.Where(t => !t.IsDeleted).Sum(t => t.Price);
            client.AmountPaid = client.AmountPaid;
            client.TotalDiscountAmount = client.Transactions.Where(t => !t.IsDeleted).Sum(t => t.DiscountAmount);
            client.TotalAmount = client.Transactions.Where(t => !t.IsDeleted).Sum(t => t.TotalAmount);
            client.TotalRemainingAmount = client.TotalAmount - client.AmountPaid;
            client.TotalMortgageAmount = client.Transactions.Where(t => !t.IsDeleted).Sum(t => t.TotalCageMortgageAmount);
            client.TotalMortgageAmountPaid = client.TotalMortgageAmountPaid;
            client.TotalRemainingMortgageAmount = client.TotalMortgageAmount - client.TotalMortgageAmountPaid;

            var today = DateOnly.FromDateTime(DateTime.Now);
            var numberOfTransactionsInMonth = await context.ClientTransactions
                .Where(c => c.ClientId == client.Id && !c.IsDeleted)
                .Where(c => c.Date.Year == today.Year && c.Date.Month == today.Month)
                .CountAsync();

            if (numberOfTransactionsInMonth >= 2)
                client.Status = Status.Active;
            else
                client.Status = Status.InActive;

            return client;
        }
        public async Task<RClientDto> UpdateClientTransactionPayments(int clientId, decimal paidAmount, decimal mortgageAmount)
        {
            var client = context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefault(c => c.Id == clientId) ?? throw new Exception("The client doesn't exist!");

            if (client.AmountPaid == client.TotalAmount && client.TotalRemainingAmount == 0)
                throw new Exception("The client's total amount was paid!");
            if (client.TotalMortgageAmount == client.TotalMortgageAmountPaid && client.TotalRemainingMortgageAmount == 0)
                throw new Exception("The client's total mortgage amount was paid!");
            if (paidAmount > client.TotalRemainingAmount)
                throw new Exception("The amount paid can't be greater than the remaining amount!");
            if (mortgageAmount > client.TotalRemainingMortgageAmount)
                throw new Exception("The mortgage amount can't be greater than the remaining mortgage amount!");

            client.TotalRemainingAmount -= paidAmount;
            client.AmountPaid += paidAmount;
            client.TotalRemainingMortgageAmount -= mortgageAmount;
            client.TotalMortgageAmountPaid += mortgageAmount;
            context.Clients.Update(client);
            await context.SaveChangesAsync();
            return client.Map<Client, RClientDto>();
        }
        public async Task<RClientDto> UpdateClientAsync(int id, WClientDto dto)
        {
            var existingClient = await context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception($"Client with id:({id}) does not exist!");

            dto.Map(existingClient);
            context.Clients.Update(existingClient);
            await context.SaveChangesAsync();
            return existingClient.Map<Client, RClientDto>();
        }
        public async Task<bool> DeleteClientAsync(int id)
        {
            var existingClient = await context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id) ?? 
                throw new Exception($"Client with id:({id}) does not exist!");

            if (existingClient.TotalRemainingAmount != 0)
                throw new Exception($"The client has remaining amount equal {existingClient.TotalRemainingAmount}, can't be deleted!");
            if (existingClient.TotalRemainingMortgageAmount != 0)
                throw new Exception($"The client has remaining mortgage amount equal {existingClient.TotalRemainingMortgageAmount} , can't be deleted!");

            existingClient.IsDeleted = true;
            context.Clients.Update(existingClient);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
