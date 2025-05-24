using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.CashTransaction;
using Shader.Data.Dtos.Client;
using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.DTOs.ClientPayment;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class ClientService(ShaderContext context) : IClientService
    {
        public async Task<IEnumerable<RAllClientsDto>> GetAllClientsAsync()
        {
            var clients =  await context.Clients
                            .Where(c => !c.IsDeleted)
                            .OrderBy(c => c.Name)
                            .ToListAsync();

            return clients.MapToRAllClientsDto();
        }

        public async Task<IEnumerable<RAllClientsDto>> GetAllClientsWithNameAsync(string name)
        {
            var clients = await context.Clients
                        .Where(c => !c.IsDeleted)
                        .Where(c =>c.Name.ToLower().Contains(name.ToLower()))
                        .OrderBy(c => c.Name)
                        .ToListAsync();

            return clients.MapToRAllClientsDto();
        }

        public async Task<RClientDto> GetClientByIdAsync(int id)
        {
            var client = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) ??
                throw new Exception($"Client with id:({id}) does not exist!");

            return client.MapToRClientDto();
        }

        public async Task<RClientDto> AddClientAsync(WClientDto dto)
        {
            if (await context.Clients.Where(c => !c.IsDeleted).AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower()))
                throw new Exception($"Client with name: ({dto.Name}) already exists!");

            var client = dto.MapToClient();
            client.Status = ClientStatus.InActive;
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
            return client.MapToRClientDto();
        }

        public async Task<RClientDto> UpdateClientAsync(int id, WClientDto dto)
        {
            var existingClient = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) ??
                throw new Exception($"Client with id:({id}) does not exist!");

            var nameFlag = context.Clients
                .Where(c => !c.IsDeleted && c.Name.ToLower() == dto.Name.ToLower())
                .Count() >= 1 && existingClient.Name.ToLower() != dto.Name.ToLower();

            if (nameFlag)
                throw new Exception($"Client with name: ({dto.Name}) already exists!");

            dto.MapToClient(existingClient);
            context.Clients.Update(existingClient);
            await context.SaveChangesAsync();
            return existingClient.MapToRClientDto();
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var existingClient = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted) ??
                throw new Exception($"Client with id:({id}) does not exist!");

            if (existingClient.TotalRemainingAmount != 0)
                throw new Exception($"The client has remaining amount equal {existingClient.TotalRemainingAmount}, can't be deleted!");
            if (existingClient.TotalRemainingMortgageAmount != 0)
                throw new Exception($"The client has remaining mortgage amount equal {existingClient.TotalRemainingMortgageAmount} , can't be deleted!");

            existingClient.IsDeleted = true;
            context.Clients.Update(existingClient);
            return await context.SaveChangesAsync() > 0;
        }

        public Client UpdateClientWithIncreaseInTransaction(Client client, ClientTransaction transaction)
        {
            client.Price += transaction.Price;
            //client.TotalAmount += transaction.Price - transaction.DiscountAmount;
            //client.TotalRemainingAmount = client.TotalAmount - client.AmountPaid;
            client.TotalDiscountAmount += transaction.DiscountAmount;
            client.TotalMortgageAmount += transaction.TotalCageMortgageAmount;
            //client.TotalRemainingMortgageAmount = client.TotalMortgageAmount - client.TotalMortgageAmountPaid;

            var today = DateOnly.FromDateTime(DateTime.Now);
            var numberOfTransactionsInMonth = context.ClientTransactions
                .Where(c => c.ClientId == client.Id && !c.IsDeleted)
                .Where(c => c.Date.Year == today.Year && c.Date.Month == today.Month)
                .Count();

            if (numberOfTransactionsInMonth >= 2)
                client.Status = ClientStatus.Active;
            else
                client.Status = ClientStatus.InActive;
            context.Clients.Update(client);
            return client;
        }
        public Client UpdateClientWithDecreaseInTransaction(Client client, ClientTransaction transaction)
        {
            client.Price -= transaction.Price;
            //client.TotalAmount -= transaction.TotalAmount;
            //client.TotalRemainingAmount = client.TotalAmount - client.AmountPaid;
            client.TotalDiscountAmount -= transaction.DiscountAmount;
            client.TotalMortgageAmount -= transaction.TotalCageMortgageAmount;
            //client.TotalRemainingMortgageAmount = client.TotalMortgageAmount - client.TotalMortgageAmountPaid;

            var today = DateOnly.FromDateTime(DateTime.Now);
            var numberOfTransactionsInMonth =  context.ClientTransactions
                .Where(c => c.ClientId == client.Id && !c.IsDeleted)
                .Where(c => c.Date.Year == today.Year && c.Date.Month == today.Month)
                .Count();

            if (numberOfTransactionsInMonth >= 2)
                client.Status = ClientStatus.Active;
            else
                client.Status = ClientStatus.InActive;

            context.Clients.Update(client);
            return client;
        }
        public Client UpdateClientWithIncreaseInPayment(Client existingClient, WClientPaymentDto paymentDto)
        {
            if (paymentDto.PaidAmount == 0 && paymentDto.MortgageAmount == 0)
                throw new Exception($"Please enter an amount greater than zero!!");
                if (paymentDto.PaidAmount < 0 || paymentDto.MortgageAmount < 0)
                throw new Exception($"Payment amount or mortgage amount can't be less than zero.");
            if (existingClient.TotalRemainingAmount == 0 && paymentDto.PaidAmount > 0)
                throw new Exception($"Client unpaid remaining amount equal ({existingClient.TotalRemainingAmount}).");
            if (existingClient.TotalRemainingMortgageAmount == 0 && paymentDto.MortgageAmount > 0)
                throw new Exception($"Client unpaid remaining mortgage amount equal ({existingClient.TotalRemainingMortgageAmount}).");
            if (paymentDto.PaidAmount > existingClient.TotalRemainingAmount)
                throw new Exception($"Paid amount ({paymentDto.PaidAmount}) exceeds the remaining amount ({existingClient.TotalRemainingAmount}).");
            if (paymentDto.MortgageAmount > existingClient.TotalRemainingMortgageAmount)
                throw new Exception($"Paid mortgage amount ({paymentDto.MortgageAmount}) exceeds the remaining mortgage amount ({existingClient.TotalRemainingMortgageAmount}).");

            existingClient.AmountPaid += paymentDto.PaidAmount;
            //existingClient.TotalRemainingAmount = existingClient.TotalAmount - existingClient.AmountPaid;
            existingClient.TotalMortgageAmountPaid += paymentDto.MortgageAmount;
            //existingClient.TotalRemainingMortgageAmount = existingClient.TotalMortgageAmount - existingClient.TotalMortgageAmountPaid;
            context.Clients.Update(existingClient);
            return existingClient;
        }
        public Client UpdateClientWithDecreaseInPayment(Client existingClient, ClientPayment payment)
        {
            existingClient.AmountPaid -= payment.PaidAmount;
            //existingClient.TotalRemainingAmount = existingClient.TotalAmount - existingClient.AmountPaid;
            existingClient.TotalMortgageAmountPaid -= payment.MortgageAmount;
            //existingClient.TotalRemainingMortgageAmount = existingClient.TotalMortgageAmount - existingClient.TotalMortgageAmountPaid;
            context.Clients.Update(existingClient);
            return existingClient;
        }

       
    }
}
