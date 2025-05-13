using Shader.Data.DTOs.MerchantPayment;
using Shader.Data.DTOs.ShaderSeller;

namespace Shader.Services.Abstraction
{
    public interface IMerchantPaymentService
    {
        Task<IEnumerable<RMerchantPaymentDto>> GetAllPaymentsAsync();
        Task<IEnumerable<RMerchantPaymentDto>> GetAllPaymentsByDateAsync(DateOnly date);
        Task<IEnumerable<RMerchantPaymentDto>> GetAllPaymentsByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<RMerchantPaymentDto>> GetPaymentsByMerchantIdAsync(int merchantId);
        Task<RMerchantPaymentDto> GetPaymentByIdAsync(int id);
        Task<RMerchantPaymentDto> CreatePaymentAsync(WMerchantPaymentDto payment);
        Task<RMerchantPaymentDto> UpdatePaymentAsync(int id, WMerchantPaymentDto payment);
        Task<bool> DeletePaymentAsync(int id);
    }
}
