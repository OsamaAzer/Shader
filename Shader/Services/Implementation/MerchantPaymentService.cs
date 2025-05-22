using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.MerchantPayment;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class MerchantPaymentService(ShaderContext context, IMerchantService merchantService) : IMerchantPaymentService
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
            var merchant = await context.Merchants
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == paymentDto.MerchantId) ??
            throw new Exception($"Merchant with ID {paymentDto.MerchantId} not found.");
            var newPayment = paymentDto.ToMerchantPayment();

            if (paymentDto.PaidAmount < 0 || paymentDto.MortgageAmount < 0)
                throw new Exception($"Payment amount or mortgage amount can't be less than zero.");
            if (merchant.CurrentAmountBalance == 0 && paymentDto.PaidAmount > 0)
                throw new Exception($"Merchant current balance equal {merchant.CurrentAmountBalance}.");
            if (merchant.CurrentMortgageAmountBalance == 0 && paymentDto.MortgageAmount > 0)
                throw new Exception($"Merchant current mortgage balance equal {merchant.CurrentMortgageAmountBalance}.");

            if (merchant.CurrentAmountBalance < 0 || merchant.CurrentMortgageAmountBalance < 0)
            {
                if (merchant.CurrentAmountBalance < 0 && paymentDto.PaidAmount > merchant.CurrentAmountBalance * -1)
                {
                    throw new Exception($"Payment amount exceeds the remaining amount {merchant.CurrentAmountBalance}.");
                }
                else
                {
                    merchant.PurchaseAmountPaid += paymentDto.PaidAmount;
                    //merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
                }
                if (merchant.CurrentMortgageAmountBalance < 0 && paymentDto.MortgageAmount > merchant.CurrentMortgageAmountBalance * -1)
                {
                    throw new Exception($"Payment amount exceeds the remaining mortgage amount {merchant.CurrentMortgageAmountBalance}.");
                }
                else
                {
                    merchant.PurchaseTotalMortgageAmountPaid += paymentDto.MortgageAmount;
                    //merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
                }
                newPayment.TransactionType = TransactionType.PayingMoney;
            }
            if (merchant.CurrentAmountBalance > 0 || merchant.CurrentMortgageAmountBalance > 0)
            {
                if (merchant.CurrentAmountBalance > 0 && paymentDto.PaidAmount > merchant.CurrentAmountBalance)
                {
                    throw new Exception($"Payment amount exceeds the remaining amount {merchant.CurrentAmountBalance}.");
                }
                else
                {
                    merchant.SellAmountPaid += paymentDto.PaidAmount;
                    //merchant.SellTotalRemainingAmount = merchant.SellTotalAmount - merchant.SellAmountPaid;
                }
                if (merchant.CurrentMortgageAmountBalance > 0 && paymentDto.MortgageAmount > merchant.CurrentMortgageAmountBalance)
                {
                    throw new Exception($"Payment amount exceeds the remaining mortgage amount {merchant.CurrentMortgageAmountBalance}.");
                }
                else
                {
                    merchant.SellTotalMortgageAmountPaid += paymentDto.MortgageAmount;
                    //merchant.SellTotalRemainingMortgageAmount = merchant.SellTotalMortgageAmount - merchant.SellTotalMortgageAmountPaid;
                }
                newPayment.TransactionType = TransactionType.ReceivingMoney;
            }
            //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            context.Merchants.Update(merchant);
            await context.MerchantPayments.AddAsync(newPayment);
            await context.SaveChangesAsync();
            return newPayment.ToRMerchantPayment();
        }
        public async Task<RMerchantPaymentDto> PayingMoneyAsync(WMerchantPaymentDto paymentDto)
        {
            var merchant = await context.Merchants
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == paymentDto.MerchantId) ??
            throw new Exception($"Merchant with ID {paymentDto.MerchantId} not found.");
            var newPayment = paymentDto.ToMerchantPayment();

            //if (paymentDto.PaidAmount < 0 || paymentDto.MortgageAmount < 0)
            //    throw new Exception($"Payment amount or mortgage amount can't be less than zero.");
            //if (merchant.CurrentAmountBalance == 0 && paymentDto.PaidAmount > 0)
            //    throw new Exception($"Merchant current balance equal {merchant.CurrentAmountBalance}.");
            //if (merchant.CurrentMortgageAmountBalance == 0 && paymentDto.MortgageAmount > 0)
            //    throw new Exception($"Merchant current mortgage balance equal {merchant.CurrentMortgageAmountBalance}.");

            //if (merchant.CurrentAmountBalance < 0 && paymentDto.PaidAmount > merchant.CurrentAmountBalance * -1)
            //{
            //    throw new Exception($"Payment amount exceeds the remaining amount {merchant.CurrentAmountBalance}.");
            //}
            //else
            //{
            //    merchant.PurchaseAmountPaid += paymentDto.PaidAmount;
            //    merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
            //}
            //if (merchant.CurrentMortgageAmountBalance < 0 && paymentDto.MortgageAmount > merchant.CurrentMortgageAmountBalance * -1)
            //{
            //    throw new Exception($"Payment amount exceeds the remaining mortgage amount {merchant.CurrentMortgageAmountBalance}.");
            //}
            //else
            //{
            //    merchant.PurchaseTotalMortgageAmountPaid += paymentDto.MortgageAmount;
            //    merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
            //}
            merchantService.UpdateMerchantWithIncreaseInPayment(merchant, paymentDto);
            newPayment.TransactionType = TransactionType.PayingMoney;
            //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            context.Merchants.Update(merchant);
            await context.MerchantPayments.AddAsync(newPayment);
            await context.SaveChangesAsync();
            return newPayment.ToRMerchantPayment();
        }
        public async Task<RMerchantPaymentDto> ReceivingMoneyAsync(WMerchantPaymentDto paymentDto)
        {
            var merchant = await context.Merchants
                            .Where(c => !c.IsDeleted)
                            .FirstOrDefaultAsync(c => c.Id == paymentDto.MerchantId) ??
                        throw new Exception($"Merchant with ID {paymentDto.MerchantId} not found.");
            var newPayment = paymentDto.ToMerchantPayment();

            //if (paymentDto.PaidAmount < 0 || paymentDto.MortgageAmount < 0)
            //    throw new Exception($"Payment amount or mortgage amount can't be less than zero.");
            //if (merchant.CurrentAmountBalance == 0 && paymentDto.PaidAmount > 0)
            //    throw new Exception($"Merchant current balance equal {merchant.CurrentAmountBalance}.");
            //if (merchant.CurrentMortgageAmountBalance == 0 && paymentDto.MortgageAmount > 0)
            //    throw new Exception($"Merchant current mortgage balance equal {merchant.CurrentMortgageAmountBalance}.");

            //if (merchant.CurrentAmountBalance > 0 && paymentDto.PaidAmount > merchant.CurrentAmountBalance)
            //{
            //    throw new Exception($"Payment amount exceeds the remaining amount {merchant.CurrentAmountBalance}.");
            //}
            //else
            //{
            //    merchant.SellAmountPaid += paymentDto.PaidAmount;
            //    merchant.SellTotalRemainingAmount = merchant.SellTotalAmount - merchant.SellAmountPaid;
            //}
            //if (merchant.CurrentMortgageAmountBalance > 0 && paymentDto.MortgageAmount > merchant.CurrentMortgageAmountBalance)
            //{
            //    throw new Exception($"Payment amount exceeds the remaining mortgage amount {merchant.CurrentMortgageAmountBalance}.");
            //}
            //else
            //{
            //    merchant.SellTotalMortgageAmountPaid += paymentDto.MortgageAmount;
            //    merchant.SellTotalRemainingMortgageAmount = merchant.SellTotalMortgageAmount - merchant.SellTotalMortgageAmountPaid;
            //}
            merchantService.UpdateMerchantWithIncreaseInPayment(merchant, paymentDto);
            newPayment.TransactionType = TransactionType.ReceivingMoney;
            //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            context.Merchants.Update(merchant);
            await context.MerchantPayments.AddAsync(newPayment);
            await context.SaveChangesAsync();
            return newPayment.ToRMerchantPayment();
        } 
        public async Task<RMerchantPaymentDto> UpdatePaymentAsync(int id, WMerchantPaymentDto paymentDto)
        {
            var payment = await context.MerchantPayments
                .Include(payment => payment.Merchant)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id) ??
                throw new Exception($"Payment with ID {id} not found.");

            var merchant = await context.Merchants
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == paymentDto.MerchantId)??
                    throw new Exception($"Merchant with ID {paymentDto.MerchantId} not found.");

            if (paymentDto.PaidAmount < 0 || paymentDto.MortgageAmount < 0)
                throw new Exception($"Payment amount or mortgage amount can't be negative.");
            if (merchant.CurrentAmountBalance == 0 && paymentDto.PaidAmount > 0)
                throw new Exception($"Merchant current balance equal {merchant.CurrentAmountBalance}.");
            if (merchant.CurrentMortgageAmountBalance == 0 && paymentDto.MortgageAmount > 0)
                throw new Exception($"Merchant current mortgage balance equal {merchant.CurrentMortgageAmountBalance}.");

            var oldPaymentTransactionType = payment.TransactionType;
            var deletedMerchantId = payment.MerchantId;
            if (deletedMerchantId != paymentDto.MerchantId)
            {
                var deletedMerchant = await context.Merchants
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefaultAsync(c => c.Id == deletedMerchantId);
                if (deletedMerchant != null)
                {
                    if (oldPaymentTransactionType == TransactionType.PayingMoney)
                    {
                        deletedMerchant.PurchaseAmountPaid -= payment.PaidAmount;
                        //deletedMerchant.PurchaseTotalRemainingAmount = deletedMerchant.PurchaseTotalAmount - deletedMerchant.PurchaseAmountPaid;
                        deletedMerchant.PurchaseTotalMortgageAmountPaid -= payment.MortgageAmount;
                        //deletedMerchant.PurchaseTotalRemainingMortgageAmount = deletedMerchant.PurchaseTotalMortgageAmount - deletedMerchant.PurchaseTotalMortgageAmountPaid;
                    }
                    else
                    {
                        deletedMerchant.SellAmountPaid -= payment.PaidAmount;
                        //deletedMerchant.SellTotalRemainingAmount = deletedMerchant.SellTotalAmount - deletedMerchant.SellAmountPaid;
                        deletedMerchant.SellTotalMortgageAmountPaid -= payment.MortgageAmount;
                        //deletedMerchant.SellTotalRemainingMortgageAmount = deletedMerchant.SellTotalMortgageAmount - deletedMerchant.SellTotalMortgageAmountPaid;
                    }
                    //deletedMerchant.CurrentAmountBalance = deletedMerchant.SellTotalRemainingAmount - deletedMerchant.PurchaseTotalRemainingAmount;
                    //deletedMerchant.CurrentMortgageAmountBalance = deletedMerchant.SellTotalRemainingMortgageAmount - deletedMerchant.PurchaseTotalRemainingMortgageAmount;
                    context.Merchants.Update(deletedMerchant);
                }
            }
            else
            {
                if (oldPaymentTransactionType == TransactionType.PayingMoney)
                {
                    merchant.PurchaseAmountPaid -= payment.PaidAmount;
                    //merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
                    merchant.PurchaseTotalMortgageAmountPaid -= payment.MortgageAmount;
                    //merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
                }
                else
                {
                    merchant.SellAmountPaid -= payment.PaidAmount;
                    //merchant.SellTotalRemainingAmount = merchant.SellTotalAmount - merchant.SellAmountPaid;
                    merchant.SellTotalMortgageAmountPaid -= payment.MortgageAmount;
                    //merchant.SellTotalRemainingMortgageAmount = merchant.SellTotalMortgageAmount - merchant.SellTotalMortgageAmountPaid;
                }
                //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
                //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
                context.Merchants.Update(merchant);
            }
            paymentDto.ToMerchantPayment(payment);
            if (merchant.CurrentAmountBalance < 0 || merchant.CurrentMortgageAmountBalance < 0)
            {
                if (paymentDto.PaidAmount > merchant.CurrentAmountBalance * -1)
                    throw new Exception($"Payment amount exceeds the remaining amount {merchant.CurrentAmountBalance}.");
                if (paymentDto.MortgageAmount > merchant.CurrentMortgageAmountBalance * -1)
                    throw new Exception($"Payment amount exceeds the remaining mortgage amount {merchant.CurrentMortgageAmountBalance}.");
                merchant.PurchaseAmountPaid += paymentDto.PaidAmount;
                //merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
                merchant.PurchaseTotalMortgageAmountPaid += paymentDto.MortgageAmount;
                //merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
                payment.TransactionType = TransactionType.PayingMoney;
            }
            if (merchant.CurrentAmountBalance > 0 || merchant.CurrentMortgageAmountBalance > 0)
            {
                if (paymentDto.PaidAmount > merchant.CurrentAmountBalance )
                    throw new Exception($"Payment amount exceeds the remaining amount {merchant.CurrentAmountBalance}.");
                if (paymentDto.MortgageAmount > merchant.CurrentMortgageAmountBalance )
                    throw new Exception($"Payment amount exceeds the remaining mortgage amount {merchant.CurrentMortgageAmountBalance}.");
                merchant.SellAmountPaid += paymentDto.PaidAmount;
                //merchant.SellTotalRemainingAmount = merchant.SellTotalAmount - merchant.SellAmountPaid;
                merchant.SellTotalMortgageAmountPaid += paymentDto.MortgageAmount;
                //merchant.SellTotalRemainingMortgageAmount = merchant.SellTotalMortgageAmount - merchant.SellTotalMortgageAmountPaid;
                payment.TransactionType = TransactionType.ReceivingMoney;
            }
            //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            context.Merchants.Update(merchant);
             
            context.MerchantPayments.Update(payment);
            await context.SaveChangesAsync();
            return payment.ToRMerchantPayment();
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
                    //existingMerchant.PurchaseTotalRemainingAmount = existingMerchant.PurchaseTotalAmount - existingMerchant.PurchaseAmountPaid;
                    existingMerchant.PurchaseTotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                    //existingMerchant.PurchaseTotalRemainingMortgageAmount = existingMerchant.PurchaseTotalMortgageAmount - existingMerchant.PurchaseTotalMortgageAmountPaid;
                }
                else
                {
                    existingMerchant.SellAmountPaid -= existingPayment.PaidAmount;
                    //existingMerchant.SellTotalRemainingAmount = existingMerchant.SellTotalAmount - existingMerchant.SellAmountPaid;
                    existingMerchant.SellTotalMortgageAmountPaid -= existingPayment.MortgageAmount;
                    //existingMerchant.SellTotalRemainingMortgageAmount = existingMerchant.SellTotalMortgageAmount - existingMerchant.SellTotalMortgageAmountPaid;
                }

                //existingMerchant.CurrentAmountBalance = existingMerchant.SellTotalRemainingAmount - existingMerchant.PurchaseTotalRemainingAmount;
                //existingMerchant.CurrentMortgageAmountBalance = existingMerchant.SellTotalRemainingMortgageAmount - existingMerchant.PurchaseTotalRemainingMortgageAmount;
                context.Merchants.Update(existingMerchant);
            }
            existingPayment.IsDeleted = true;
            context.MerchantPayments.Update(existingPayment);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
