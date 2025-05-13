using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.MerchantPayment;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class MerchantPaymentService(ShaderContext context) : IMerchantPaymentService
    {
        public async Task<PagedResponse<RMerchantPaymentDto>> GetAllPaymentsAsync(int pageNumber, int pageSize)
        {
            var payments = await context.MerchantPayments
                .Include(payment => payment.Merchant)
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
            return payments.ToRMerchantPayments().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RMerchantPaymentDto>> GetAllPaymentsByDateAsync(DateOnly date, int pageNumber, int pageSize)
        {
            var payments = await context.MerchantPayments
                .Include(payment => payment.Merchant)
                .Where(p => !p.IsDeleted)
                .Where(p => DateOnly.FromDateTime(p.Date) == date)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
            return payments.ToRMerchantPayments().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RMerchantPaymentDto>> GetAllPaymentsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            var payments = await context.MerchantPayments
                .Include(payment => payment.Merchant)
                .Where(p => !p.IsDeleted)
                .Where(p => DateOnly.FromDateTime(p.Date) >= startDate && DateOnly.FromDateTime(p.Date) <= endDate)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
            return payments.ToRMerchantPayments().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RMerchantPaymentDto>> GetPaymentsByMerchantIdAsync(int merchantId, int pageNumber, int pageSize)
        {
            var payments = await context.MerchantPayments
                .Include(payment => payment.Merchant)
                .Where(p => !p.IsDeleted && p.MerchantId == merchantId)
                .OrderByDescending(p => p.Date)
                .ToListAsync() ??
            throw new Exception($"Payment with Merchant ID {merchantId} not found.");
            return payments.ToRMerchantPayments().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<RMerchantPaymentDto> GetPaymentByIdAsync(int id)
        {
            var payment = await context.MerchantPayments
                .Include(payment => payment.Merchant)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id) ??
            throw new Exception($"Payment with ID {id} not found.");
            return payment.ToRMerchantPayment();
        }
        public async Task<RMerchantPaymentDto> CreatePaymentAsync(WMerchantPaymentDto paymentDto)
        {
            var existingMerchant = await context.Merchants
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == paymentDto.MerchantId) ??
            throw new Exception($"Merchant with ID {paymentDto.MerchantId} not found.");
            var newPayment = paymentDto.ToMerchantPayment();

            if(existingMerchant.CurrentAmountBalance == 0)
                throw new Exception($"Merchant current balance equal {existingMerchant.CurrentAmountBalance}.");
            if (existingMerchant.CurrentMortgageAmountBalance == 0)
                throw new Exception($"Merchant current mortgage balance equal {existingMerchant.CurrentMortgageAmountBalance}.");

            if (existingMerchant.CurrentAmountBalance < 0 || existingMerchant.CurrentMortgageAmountBalance < 0)
            {
                if (paymentDto.PaidAmount > existingMerchant.CurrentAmountBalance * -1)
                    throw new Exception($"Payment amount exceeds the remaining amount {existingMerchant.CurrentAmountBalance}.");
                if (paymentDto.MortgageAmount > existingMerchant.CurrentMortgageAmountBalance * -1)
                    throw new Exception($"Payment amount exceeds the remaining mortgage amount {existingMerchant.CurrentMortgageAmountBalance}.");
                existingMerchant.PurchaseAmountPaid += paymentDto.PaidAmount;
                existingMerchant.PurchaseTotalRemainingAmount = existingMerchant.PurchaseTotalAmount - existingMerchant.PurchaseAmountPaid;
                existingMerchant.PurchaseTotalMortgageAmountPaid += paymentDto.MortgageAmount;
                existingMerchant.PurchaseTotalRemainingMortgageAmount = existingMerchant.PurchaseTotalMortgageAmount - existingMerchant.PurchaseTotalMortgageAmountPaid;
                newPayment.TransactionType = TransactionType.PayingMoney;
            }
            if (existingMerchant.CurrentAmountBalance > 0 || existingMerchant.CurrentMortgageAmountBalance > 0)
            {
                if (paymentDto.PaidAmount > existingMerchant.CurrentAmountBalance * -1)
                    throw new Exception($"Payment amount exceeds the remaining amount {existingMerchant.CurrentAmountBalance}.");
                if (paymentDto.MortgageAmount > existingMerchant.CurrentMortgageAmountBalance * -1)
                    throw new Exception($"Payment amount exceeds the remaining mortgage amount {existingMerchant.CurrentMortgageAmountBalance}.");
                existingMerchant.SellAmountPaid += paymentDto.PaidAmount;
                existingMerchant.SellTotalRemainingAmount = existingMerchant.SellTotalAmount - existingMerchant.SellAmountPaid;
                existingMerchant.SellTotalMortgageAmountPaid += paymentDto.MortgageAmount;
                existingMerchant.SellTotalRemainingMortgageAmount = existingMerchant.SellTotalMortgageAmount - existingMerchant.SellTotalMortgageAmountPaid;
                newPayment.TransactionType = TransactionType.ReceivingMoney;
            }
            existingMerchant.CurrentAmountBalance = existingMerchant.SellTotalRemainingAmount - existingMerchant.PurchaseTotalRemainingAmount;
            existingMerchant.CurrentMortgageAmountBalance = existingMerchant.SellTotalRemainingMortgageAmount - existingMerchant.PurchaseTotalRemainingMortgageAmount;
            context.Merchants.Update(existingMerchant);
            await context.MerchantPayments.AddAsync(newPayment);
            await context.SaveChangesAsync();
            return newPayment.ToRMerchantPayment();
        }
        public async Task<RMerchantPaymentDto> UpdatePaymentAsync(int id, WMerchantPaymentDto paymentDto)
        {
            var existingPayment = await context.MerchantPayments
                .Include(payment => payment.Merchant)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id) ??
            throw new Exception($"Payment with ID {id} not found.");

            var existingMerchant = await context.Merchants
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == paymentDto.MerchantId);

            var oldPaymentTransactionType = existingPayment.TransactionType;
            var oldMerchantId = existingPayment.MerchantId;
            if (oldMerchantId != paymentDto.MerchantId)
            {
                var oldMerchant = await context.Merchants
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == oldMerchantId);
                if (oldMerchant != null)
                {
                    if (oldPaymentTransactionType == TransactionType.PayingMoney)
                    {
                        oldMerchant.PurchaseAmountPaid -= existingPayment.PaidAmount;
                        oldMerchant.PurchaseTotalRemainingAmount = oldMerchant.PurchaseTotalAmount - oldMerchant.PurchaseAmountPaid;
                        oldMerchant.PurchaseTotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                        oldMerchant.PurchaseTotalRemainingMortgageAmount = oldMerchant.PurchaseTotalMortgageAmount - oldMerchant.PurchaseTotalMortgageAmountPaid;
                    }
                    else
                    {
                        oldMerchant.SellAmountPaid -= existingPayment.PaidAmount;
                        oldMerchant.SellTotalRemainingAmount = oldMerchant.SellTotalAmount - oldMerchant.SellAmountPaid;
                        oldMerchant.SellTotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                        oldMerchant.SellTotalRemainingMortgageAmount = oldMerchant.SellTotalMortgageAmount - oldMerchant.SellTotalMortgageAmountPaid;
                    }
                    oldMerchant.CurrentAmountBalance = oldMerchant.SellTotalRemainingAmount - oldMerchant.PurchaseTotalRemainingAmount;
                    oldMerchant.CurrentMortgageAmountBalance = oldMerchant.SellTotalRemainingMortgageAmount - oldMerchant.PurchaseTotalRemainingMortgageAmount;
                    context.Merchants.Update(oldMerchant);
                }
            }
            else
            {
                if (existingMerchant != null)
                {
                    if (oldPaymentTransactionType == TransactionType.PayingMoney)
                    {
                        existingMerchant.PurchaseAmountPaid -= existingPayment.PaidAmount;
                        existingMerchant.PurchaseTotalRemainingAmount = existingMerchant.PurchaseTotalAmount - existingMerchant.PurchaseAmountPaid;
                        existingMerchant.PurchaseTotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                        existingMerchant.PurchaseTotalRemainingMortgageAmount = existingMerchant.PurchaseTotalMortgageAmount - existingMerchant.PurchaseTotalMortgageAmountPaid;
                    }
                    else
                    {
                        existingMerchant.SellAmountPaid -= existingPayment.PaidAmount;
                        existingMerchant.SellTotalRemainingAmount = existingMerchant.SellTotalAmount - existingMerchant.SellAmountPaid;
                        existingMerchant.SellTotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                        existingMerchant.SellTotalRemainingMortgageAmount = existingMerchant.SellTotalMortgageAmount - existingMerchant.SellTotalMortgageAmountPaid;
                    }
                    existingMerchant.CurrentAmountBalance = existingMerchant.SellTotalRemainingAmount - existingMerchant.PurchaseTotalRemainingAmount;
                    existingMerchant.CurrentMortgageAmountBalance = existingMerchant.SellTotalRemainingMortgageAmount - existingMerchant.PurchaseTotalRemainingMortgageAmount;
                    context.Merchants.Update(existingMerchant);
                }
            }
            paymentDto.ToMerchantPayment(existingPayment);
            if (existingMerchant is not null)
            {
                if (existingMerchant.CurrentAmountBalance < 0 || existingMerchant.CurrentMortgageAmountBalance < 0)
                {
                    if (paymentDto.PaidAmount > existingMerchant.CurrentAmountBalance * -1)
                        throw new Exception($"Payment amount exceeds the remaining amount {existingMerchant.CurrentAmountBalance}.");
                    if (paymentDto.MortgageAmount > existingMerchant.CurrentMortgageAmountBalance * -1)
                        throw new Exception($"Payment amount exceeds the remaining mortgage amount {existingMerchant.CurrentMortgageAmountBalance}.");
                    existingMerchant.PurchaseAmountPaid += paymentDto.PaidAmount;
                    existingMerchant.PurchaseTotalRemainingAmount = existingMerchant.PurchaseTotalAmount - existingMerchant.PurchaseAmountPaid;
                    existingMerchant.PurchaseTotalMortgageAmountPaid += paymentDto.MortgageAmount;
                    existingMerchant.PurchaseTotalRemainingMortgageAmount = existingMerchant.PurchaseTotalMortgageAmount - existingMerchant.PurchaseTotalMortgageAmountPaid;
                    existingPayment.TransactionType = TransactionType.PayingMoney;
                }
                if (existingMerchant.CurrentAmountBalance > 0 || existingMerchant.CurrentMortgageAmountBalance > 0)
                {
                    if (paymentDto.PaidAmount > existingMerchant.CurrentAmountBalance )
                        throw new Exception($"Payment amount exceeds the remaining amount {existingMerchant.CurrentAmountBalance}.");
                    if (paymentDto.MortgageAmount > existingMerchant.CurrentMortgageAmountBalance )
                        throw new Exception($"Payment amount exceeds the remaining mortgage amount {existingMerchant.CurrentMortgageAmountBalance}.");
                    existingMerchant.SellAmountPaid += paymentDto.PaidAmount;
                    existingMerchant.SellTotalRemainingAmount = existingMerchant.SellTotalAmount - existingMerchant.SellAmountPaid;
                    existingMerchant.SellTotalMortgageAmountPaid += paymentDto.MortgageAmount;
                    existingMerchant.SellTotalRemainingMortgageAmount = existingMerchant.SellTotalMortgageAmount - existingMerchant.SellTotalMortgageAmountPaid;
                    existingPayment.TransactionType = TransactionType.ReceivingMoney;
                }
                existingMerchant.CurrentAmountBalance = existingMerchant.SellTotalRemainingAmount - existingMerchant.PurchaseTotalRemainingAmount;
                existingMerchant.CurrentMortgageAmountBalance = existingMerchant.SellTotalRemainingMortgageAmount - existingMerchant.PurchaseTotalRemainingMortgageAmount;
                context.Merchants.Update(existingMerchant);
            }
             
            context.MerchantPayments.Update(existingPayment);
            await context.SaveChangesAsync();
            return existingPayment.ToRMerchantPayment();
        }
        public async Task<bool> DeletePaymentAsync(int id)
        {
            var existingPayment = await context.MerchantPayments
                .Include(payment => payment.Merchant)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id) ??
            throw new Exception($"Payment with ID {id} not found.");

            var existingMerchant = await context.Merchants
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == existingPayment.MerchantId);

            if (existingMerchant != null)
            {
                if (existingPayment.TransactionType == Enums.TransactionType.PayingMoney)
                {
                    existingMerchant.PurchaseAmountPaid -= existingPayment.PaidAmount;
                    existingMerchant.PurchaseTotalRemainingAmount = existingMerchant.PurchaseTotalAmount - existingMerchant.PurchaseAmountPaid;
                    existingMerchant.PurchaseTotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                    existingMerchant.PurchaseTotalRemainingMortgageAmount = existingMerchant.PurchaseTotalMortgageAmount - existingMerchant.PurchaseTotalMortgageAmountPaid;
                }
                else
                {
                    existingMerchant.SellAmountPaid -= existingPayment.PaidAmount;
                    existingMerchant.SellTotalRemainingAmount = existingMerchant.SellTotalAmount - existingMerchant.SellAmountPaid;
                    existingMerchant.SellTotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                    existingMerchant.SellTotalRemainingMortgageAmount = existingMerchant.SellTotalMortgageAmount - existingMerchant.SellTotalMortgageAmountPaid;
                }

                existingMerchant.CurrentAmountBalance = existingMerchant.SellTotalRemainingAmount - existingMerchant.PurchaseTotalRemainingAmount;
                existingMerchant.CurrentMortgageAmountBalance = existingMerchant.SellTotalRemainingMortgageAmount - existingMerchant.PurchaseTotalRemainingMortgageAmount;
                context.Merchants.Update(existingMerchant);
            }
            existingPayment.IsDeleted = true;
            context.MerchantPayments.Update(existingPayment);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
