using Shader.Data.DTOs.MerchantPayment;
using Shader.Data.DTOs.ShaderSeller;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IMerchantPaymentService
    {
        Task<PagedResponse<RMerchantPaymentDto>> GetAllPaymentsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RMerchantPaymentDto>> GetAllPaymentsByDateAsync(DateOnly date, int pageNumber, int pageSize);
        Task<PagedResponse<RMerchantPaymentDto>> GetAllPaymentsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<PagedResponse<RMerchantPaymentDto>> GetPaymentsByMerchantIdAsync(int merchantId, int pageNumber, int pageSize);
        Task<RMerchantPaymentDto> GetPaymentByIdAsync(int id);
        Task<RMerchantPaymentDto> CreatePaymentAsync(WMerchantPaymentDto payment);
        Task<RMerchantPaymentDto> UpdatePaymentAsync(int id, WMerchantPaymentDto payment);
        Task<bool> DeletePaymentAsync(int id);
    }
}
