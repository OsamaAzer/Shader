using Humanizer;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Shader.Data;
using Shader.Data.DTOs.MerchantPayment;
using Shader.Data.DTOs.ShaderSeller;
using Shader.Data.DTOs.ShaderTransaction;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class MerchantService(ShaderContext context) : IMerchantService
    {
        public async Task<PagedResponse<RMerchantDto>> GetAllMerchantsAsync(int pageNumber, int pageSize)
        {
            var sellers = await context.Merchants
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            return sellers.ToDtos<Merchant, RMerchantDto>().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RMerchantDto>> GetAllMerchantsWithNameAsync(string name, int pageNumber, int pageSize)
        {
            var sellers = await context.Merchants
                .Where(s => !s.IsDeleted)
                .Where(s => s.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
            return sellers.ToDtos<Merchant, RMerchantDto>().CreatePagedResponse(pageNumber, pageSize);    
        }
        public async Task<Merchant> GetMerchantByIdAsync(int id)
        {
            var merchant = await context.Merchants
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == id) ??
                throw new Exception($"Seller with ID {id} not found.");
            return merchant;
        }
        public async Task<RMerchantDto> CreateMerchantAsync(WMerchantDto merchantDto)
        {
            if (context.Merchants.Where(m => !m.IsDeleted).Any(m => m.Name.ToLower() == merchantDto.Name.ToLower()))
                throw new Exception($"Merchant with name {merchantDto.Name} already exists!!");

            var merchant = merchantDto.ToEntity<WMerchantDto, Merchant>();
            await context.Merchants.AddAsync(merchant);
            await context.SaveChangesAsync();
            return merchant.ToDto<Merchant, RMerchantDto>();
        }
        public async Task<RMerchantDto> UpdateMerchantAsync(int id, WMerchantDto merchantDto)
        {
            var merchant = await context.Merchants
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == id) ??
                throw new Exception($"Merchant with ID {id} not found.");

            var nameFlag = context.Merchants
                .Where(f => !f.IsDeleted && f.Name.ToLower() == merchantDto.Name.ToLower())
                .Count() >= 1 && merchant.Name.ToLower() != merchantDto.Name.ToLower();
            if (nameFlag) throw new Exception($"Merchant with name {merchantDto.Name} already exists!!");

            merchantDto.ToEntity(merchant);
            context.Merchants.Update(merchant);
            await context.SaveChangesAsync();
            return merchant.ToDto<Merchant, RMerchantDto>();
        }
        public  Merchant UpdateMerchantWithIncreaseInTransaction(Merchant merchant, MerchantTransaction transaction)
        {
            merchant.PurchasePrice += transaction.Price;
            merchant.PurchaseTotalDiscountAmount += transaction.DiscountAmount;
            //merchant.PurchaseTotalAmount = merchant.PurchasePrice - merchant.PurchaseTotalDiscountAmount;
            //merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
            merchant.PurchaseTotalMortgageAmount += transaction.TotalCageMortgageAmount;
            //merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
            //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            context.Merchants.Update(merchant);
            return merchant;
        }
        public  Merchant UpdateMerchantWithDecreaseInTransaction(Merchant merchant, MerchantTransaction transaction)
        {
            merchant.PurchasePrice -= transaction.Price;
            merchant.PurchaseTotalDiscountAmount -= transaction.DiscountAmount;
            //merchant.PurchaseTotalAmount = merchant.PurchasePrice - merchant.PurchaseTotalDiscountAmount;
            //merchant.PurchaseTotalRemainingAmount = merchant.PurchaseTotalAmount - merchant.PurchaseAmountPaid;
            merchant.PurchaseTotalMortgageAmount -= transaction.TotalCageMortgageAmount;
            //merchant.PurchaseTotalRemainingMortgageAmount = merchant.PurchaseTotalMortgageAmount - merchant.PurchaseTotalMortgageAmountPaid;
            //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            context.Merchants.Update(merchant);
            return merchant;
        }
        public  Merchant UpdateMerchantWithIncreaseInPayment(Merchant merchant, WMerchantPaymentDto paymentDto)
        {
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
            }
            //merchant.CurrentAmountBalance = merchant.SellTotalRemainingAmount - merchant.PurchaseTotalRemainingAmount;
            //merchant.CurrentMortgageAmountBalance = merchant.SellTotalRemainingMortgageAmount - merchant.PurchaseTotalRemainingMortgageAmount;
            return merchant;
        }
        public  Merchant UpdateMerchantWithDecreaseInPayment(Merchant merchant, WMerchantPaymentDto paymentDto)
        {

            return merchant;
        }
        public async Task<bool> DeleteMerchantAsync(int id)
        {
            var merchant = await context.Merchants
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == id) ??
                throw new Exception($"Seller with ID {id} not found.");

            if (merchant.CurrentAmountBalance > 0 || merchant.CurrentAmountBalance < 0)
                throw new Exception($"The merchant has a balance equal {merchant.CurrentAmountBalance}, please settle the balance before deleting.");
            if (merchant.CurrentMortgageAmountBalance > 0 || merchant.CurrentMortgageAmountBalance < 0)
                throw new Exception($"The merchant has a mortgage balanceequal {merchant.CurrentMortgageAmountBalance}, please settle the balance before deleting.");

            merchant.IsDeleted = true;
            context.Merchants.Update(merchant);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
