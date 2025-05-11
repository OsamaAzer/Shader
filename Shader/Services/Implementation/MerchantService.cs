using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.ShaderSeller;
using Shader.Data.Entities;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class MerchantService(ShaderContext context) : IMerchantService
    {
        public async Task<IEnumerable<RMerchantDto>> GetAllMerchantsAsync()
        {
            var sellers = await context.Merchants
                .Where(s => !s.IsDeleted)
                .ToListAsync();

            return sellers.ToDtos<Merchant, RMerchantDto>().ToList();
        }
        public async Task<IEnumerable<RMerchantDto>> GetAllMerchantsWithNameAsync(string name)
        {
            var sellers = await context.Merchants
                .Where(s => !s.IsDeleted)
                .Where(s => s.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
            return sellers.ToDtos<Merchant, RMerchantDto>().ToList();
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
        public async Task<bool> DeleteMerchantAsync(int id)
        {
            var merchant = await context.Merchants
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == id) ??
                throw new Exception($"Seller with ID {id} not found.");

            merchant.IsDeleted = true;
            context.Merchants.Update(merchant);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
