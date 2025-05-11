using Shader.Data.DTOs.ShaderSeller;
using Shader.Data.DTOs.ShaderTransaction;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IMerchantService
    {
        Task<IEnumerable<RMerchantDto>> GetAllMerchantsAsync();
        Task<IEnumerable<RMerchantDto>> GetAllMerchantsWithNameAsync(string name);
        Task<Merchant> GetMerchantByIdAsync(int id);
        Task<RMerchantDto> CreateMerchantAsync(WMerchantDto sellerDto);
        Task<RMerchantDto> UpdateMerchantAsync(int id, WMerchantDto sellerDto);
        Task<bool> DeleteMerchantAsync(int id); // for hard delete
    }
}
