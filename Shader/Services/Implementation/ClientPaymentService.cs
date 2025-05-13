using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.ClientPayment;
using Shader.Data.Entities;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class ClientPaymentService(ShaderContext context) : IClientPaymentService
    {
       public async Task<PagedResponse<RClientPaymentDto>> GetAllPaymentsAsync(int pageNumber, int pageSize)
       {
            var payments = await context.ClientPayments
                .Include(payment => payment.Client)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
            return payments.ToRClientPayments().CreatePagedResponse(pageNumber, pageSize);
       }
       public async Task<PagedResponse<RClientPaymentDto>> GetAllPaymentsByDateAsync(DateOnly date, int pageNumber, int pageSize)
       {
            var payments = await context.ClientPayments
                .Include(payment => payment.Client)
                .Where(p => !p.IsDeleted)
                .Where(p => DateOnly.FromDateTime(p.Date) == date)
                .ToListAsync();
            return payments.ToRClientPayments().CreatePagedResponse(pageNumber, pageSize);
       }
       public async Task<PagedResponse<RClientPaymentDto>> GetAllPaymentsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
       {
            var payments = await context.ClientPayments
                .Include(payment => payment.Client)
                .Where(p => !p.IsDeleted)
                .Where(p => DateOnly.FromDateTime(p.Date) >= startDate && DateOnly.FromDateTime(p.Date) <= endDate)
                .ToListAsync();
            return payments.ToRClientPayments().CreatePagedResponse(pageNumber, pageSize);
       }
       public async Task<PagedResponse<RClientPaymentDto>> GetPaymentsByClientIdAsync(int clientId, int pageNumber, int pageSize)
       {
            var payment = await context.ClientPayments
                .Include(payment => payment.Client)
                    .Where(p => !p.IsDeleted && p.ClientId == clientId)
                    .ToListAsync() ??
                throw new Exception($"Payment with Client ID {clientId} not found.");
            return payment.ToRClientPayments().CreatePagedResponse(pageNumber, pageSize);
       }
       public async Task<RClientPaymentDto> GetPaymentByIdAsync(int id)
        {
            var payment = await context.ClientPayments
                    .Include(payment => payment.Client)
                    .Where(p => !p.IsDeleted)
                    .FirstOrDefaultAsync(p => p.Id == id) ??
                throw new Exception($"Payment with ID {id} not found.");
            return payment.ToRClientPayment();
        }
       public async Task<RClientPaymentDto> CreatePaymentAsync(WClientPaymentDto paymentDto)
        {
            var existingClient = await context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == paymentDto.ClientId) ??
            throw new Exception($"Client with ID {paymentDto.ClientId} not found.");

            if (paymentDto.PaidAmount < 0 || paymentDto.MortgageAmount < 0)
                throw new Exception($"Payment amount or mortgage amount can't be negative.");
            if (existingClient.TotalRemainingAmount == 0 && paymentDto.PaidAmount > 0)
                throw new Exception($"Client unpaid remaining amount equal {existingClient.TotalRemainingAmount}.");
            if (existingClient.TotalRemainingMortgageAmount == 0 && paymentDto.MortgageAmount > 0)
                throw new Exception($"Client unpaid remaining mortgage amount equal {existingClient.TotalRemainingMortgageAmount}.");
            if (paymentDto.PaidAmount > existingClient.TotalRemainingAmount)
                throw new Exception($"Payment amount exceeds the remaining amount {existingClient.TotalRemainingAmount}.");
            if (paymentDto.MortgageAmount > existingClient.TotalRemainingMortgageAmount)
                throw new Exception($"Payment amount exceeds the remaining mortgage amount {existingClient.TotalRemainingMortgageAmount}.");

            existingClient.AmountPaid += paymentDto.PaidAmount;
            existingClient.TotalRemainingAmount = existingClient.TotalAmount - existingClient.AmountPaid;
            existingClient.TotalMortgageAmountPaid += paymentDto.MortgageAmount;
            existingClient.TotalMortgageAmountPaid = existingClient.TotalMortgageAmount - existingClient.TotalMortgageAmountPaid;
            var clientPayment = paymentDto.ToClientPayment();
            await context.ClientPayments.AddAsync(clientPayment);
            await context.SaveChangesAsync();
            return clientPayment.ToRClientPayment();
        }
       public async Task<RClientPaymentDto> UpdatePaymentAsync(int id, WClientPaymentDto paymentDto)
       {
            var existingPayment = await context.ClientPayments
                .Include(payment => payment.Client)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id) ??
                throw new Exception($"Payment with ID {id} not found.");

            var oldClientId = existingPayment.ClientId;
            if (oldClientId != paymentDto.ClientId)
            {
                var oldClient = await context.Clients
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == oldClientId);
                
                if (oldClient != null)
                {
                    oldClient.AmountPaid -= existingPayment.PaidAmount;
                    oldClient.TotalRemainingAmount = oldClient.TotalAmount - oldClient.AmountPaid;
                    oldClient.TotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                    oldClient.TotalRemainingMortgageAmount = oldClient.TotalMortgageAmount - oldClient.TotalMortgageAmountPaid;
                    context.Clients.Update(oldClient);
                }
            }
            else
            {
                var existingClient = await context.Clients
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == paymentDto.ClientId);
                if(existingClient != null)
                {
                    if (paymentDto.PaidAmount > existingClient.TotalRemainingAmount)
                        throw new Exception($"Payment amount exceeds the remaining amount {existingClient.TotalRemainingAmount}.");
                    if (paymentDto.MortgageAmount > existingClient.TotalRemainingMortgageAmount)
                        throw new Exception($"Payment amount exceeds the remaining mortgage amount {existingClient.TotalRemainingMortgageAmount}.");
                    existingClient.AmountPaid -= existingPayment.PaidAmount;
                    existingClient.TotalRemainingAmount = existingClient.TotalAmount - existingClient.AmountPaid;
                    existingClient.TotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                    existingClient.TotalRemainingMortgageAmount = existingPayment.MortgageAmount - existingClient.TotalMortgageAmountPaid;

                    existingClient.AmountPaid += paymentDto.PaidAmount;
                    existingClient.TotalRemainingAmount = existingClient.TotalAmount - existingClient.AmountPaid;
                    existingClient.TotalMortgageAmountPaid += paymentDto.MortgageAmount;
                    existingClient.TotalRemainingMortgageAmount = existingClient.TotalMortgageAmount - existingClient.TotalMortgageAmountPaid;
                    context.Clients.Update(existingClient);
                }
            }
            paymentDto.ToClientPayment(existingPayment);
            context.ClientPayments.Update(existingPayment);
            await context.SaveChangesAsync();
            return existingPayment.ToRClientPayment();
       }
       public async Task<bool> DeletePaymentAsync(int id)
       {
            var payment = await context.ClientPayments
                .Include(payment => payment.Client)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id) ??
                throw new Exception($"Payment with ID {id} not found.");

            var client = await context.Clients
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == payment.ClientId);

            if (client != null)
            {
                client.AmountPaid -= payment.PaidAmount;
                client.TotalRemainingAmount = client.TotalAmount - client.AmountPaid;
                client.TotalMortgageAmountPaid -= payment.MortgageAmount;
                client.TotalRemainingMortgageAmount = client.TotalMortgageAmountPaid - client.TotalMortgageAmountPaid;
                context.Clients.Update(client);
            }
            payment.IsDeleted = true;
            context.ClientPayments.Update(payment);
            await context.SaveChangesAsync();
            return true;
       }
    }
}
