using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.ClientPayment;
using Shader.Data.Entities;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class ClientPaymentService(ShaderContext context, IClientService clientService) : IClientPaymentService
    {
       public async Task<PagedResponse<RClientPaymentDto>> GetAllPaymentsAsync(int pageNumber, int pageSize)
       {
            var payments = await context.ClientPayments
                .Include(payment => payment.Client)
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.Date)
                .ToListAsync();

            return payments.ToRClientPayments().CreatePagedResponse(pageNumber, pageSize);
       }

       public async Task<PagedResponse<RClientPaymentDto>> GetAllPaymentsByDateAsync(DateOnly date, int pageNumber, int pageSize)
       {
            var payments = await context.ClientPayments
                .Include(payment => payment.Client)
                .Where(p => !p.IsDeleted)
                .Where(p => DateOnly.FromDateTime(p.Date) == date)
                .OrderByDescending(p => p.Date)
                .ToListAsync();

            return payments.ToRClientPayments().CreatePagedResponse(pageNumber, pageSize);
       }

       public async Task<PagedResponse<RClientPaymentDto>> GetAllPaymentsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
       {
            if (startDate == default || endDate == default)
                throw new Exception("Start date and end date are both required.");

            if (startDate > endDate)
                throw new Exception($"Start date {startDate} can't be greater than end date {endDate}.");

            var payments = await context.ClientPayments
                .Include(payment => payment.Client)
                .Where(p => !p.IsDeleted)
                .Where(p => DateOnly.FromDateTime(p.Date) >= startDate && DateOnly.FromDateTime(p.Date) <= endDate)
                .OrderByDescending(p => p.Date)
                .ToListAsync();

            return payments.ToRClientPayments().CreatePagedResponse(pageNumber, pageSize);
       }

       public async Task<PagedResponse<RClientPaymentDto>> GetPaymentsByClientIdAsync(int clientId, int pageNumber, int pageSize)
       {
            var existingClient = await context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == clientId) ??
                throw new Exception($"Client with ID {clientId} not found.");

            var payments = await context.ClientPayments
                .Include(payment => payment.Client)
                .Where(p => !p.IsDeleted && p.ClientId == clientId)
                .OrderByDescending(p => p.Date)
                .ToListAsync();

            return payments.ToRClientPayments().CreatePagedResponse(pageNumber, pageSize);
       }

       public async Task<RClientPaymentDto> GetPaymentByIdAsync(int id)
       {
            var payment = await context.ClientPayments
                    .Include(payment => payment.Client)
                    .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted) ??
                throw new Exception($"Payment with ID {id} not found.");

            return payment.ToRClientPayment();
       }

       public async Task<RClientPaymentDto> CreatePaymentAsync(WClientPaymentDto paymentDto)
       {
            var existingClient = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == paymentDto.ClientId && !c.IsDeleted) ??
            throw new Exception($"Client with ID {paymentDto.ClientId} not found.");

            clientService.UpdateClientWithIncreaseInPayment(existingClient, paymentDto);
            var clientPayment = paymentDto.ToClientPayment();
            await context.ClientPayments.AddAsync(clientPayment);
            await context.SaveChangesAsync();
            return clientPayment.ToRClientPayment();
       }

       public async Task<RClientPaymentDto> UpdatePaymentAsync(int id, WClientPaymentDto paymentDto)
       {
            var payment = await context.ClientPayments
                .Include(payment => payment.Client)
                .FirstOrDefaultAsync(c => c.Id == paymentDto.ClientId && !c.IsDeleted) ??
                throw new Exception($"Payment with ID {id} not found.");

            var client = await context.Clients
                    .FirstOrDefaultAsync(c => c.Id == paymentDto.ClientId && !c.IsDeleted) ??
                    throw new Exception($"Client with ID {paymentDto.ClientId} not found.");

            var removedClientId = payment.ClientId;
            if (removedClientId != paymentDto.ClientId)
            {
                var removedClient = await context.Clients
                    .FirstOrDefaultAsync(c => c.Id == removedClientId && !c.IsDeleted);
                
                if (removedClient != null)
                    clientService.UpdateClientWithDecreaseInPayment(removedClient, payment);
            }
            else
            {
                clientService.UpdateClientWithDecreaseInPayment(client, payment);
                clientService.UpdateClientWithIncreaseInPayment(client, paymentDto);
            }
            paymentDto.ToClientPayment(payment);
            context.ClientPayments.Update(payment);
            await context.SaveChangesAsync();
            return payment.ToRClientPayment();
       }

       public async Task<bool> DeletePaymentAsync(int id)
       {
            var payment = await context.ClientPayments
                .Include(payment => payment.Client)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted) ??
                throw new Exception($"Payment with ID {id} not found.");

            var client = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == payment.ClientId && !c.IsDeleted);

            if (client != null)
                clientService.UpdateClientWithDecreaseInPayment(client, payment);

            payment.IsDeleted = true;
            context.ClientPayments.Update(payment);
            return await context.SaveChangesAsync() > 0;
       }
    }
}
