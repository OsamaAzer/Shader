using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.ShaderSeller;
using Shader.Data.Entities;
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
                throw new Exception($"Seller with ID {id} not found.");

            merchantDto.ToEntity(merchant);
            context.Merchants.Update(merchant);
            await context.SaveChangesAsync();
            return merchant.ToDto<Merchant, RMerchantDto>();
        }
        public async Task<Merchant> UpdateMerchantAggregatesAsync(Merchant merchant)
        {
            var existingMerchant = await context.Merchants
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == merchant.Id) ??
                throw new Exception($"Merchant with ID {merchant.Id} not found.");
            
            
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
