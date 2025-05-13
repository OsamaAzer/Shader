using Shader.Data.DTOs.ShaderSeller;
using Shader.Data.DTOs.ShaderTransaction;
using Shader.Data.Entities;
using Shader.Helpers;
using System.Drawing.Printing;

namespace Shader.Services.Abstraction
{
    public interface IMerchantService
    {
        Task<PagedResponse<RMerchantDto>> GetAllMerchantsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RMerchantDto>> GetAllMerchantsWithNameAsync(string name, int pageNumber, int pageSize);
        Task<Merchant> GetMerchantByIdAsync(int id);
        Task<RMerchantDto> CreateMerchantAsync(WMerchantDto sellerDto);
        Task<RMerchantDto> UpdateMerchantAsync(int id, WMerchantDto sellerDto);
        Task<Merchant> UpdateMerchantAggregatesAsync(Merchant merchant);
        Task<bool> DeleteMerchantAsync(int id); // for hard delete
    }
}
