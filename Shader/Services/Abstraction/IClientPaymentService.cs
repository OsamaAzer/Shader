using Shader.Data.DTOs.ClientPayment;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IClientPaymentService
    {
        Task<PagedResponse<RClientPaymentDto>> GetAllPaymentsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RClientPaymentDto>> GetAllPaymentsByDateAsync(DateOnly date, int pageNumber, int pageSize);
        Task<PagedResponse<RClientPaymentDto>> GetAllPaymentsByDateRangeAsync(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<PagedResponse<RClientPaymentDto>> GetPaymentsByClientIdAsync(int clientId, int pageNumber, int pageSize);
        Task<RClientPaymentDto> GetPaymentByIdAsync(int id);
        Task<RClientPaymentDto> CreatePaymentAsync(WClientPaymentDto payment);
        Task<RClientPaymentDto> UpdatePaymentAsync(int id, WClientPaymentDto payment);
        Task<bool> DeletePaymentAsync(int id); 
    }
}
