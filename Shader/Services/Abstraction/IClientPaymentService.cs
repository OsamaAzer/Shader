using Shader.Data.DTOs.ClientPayment;

namespace Shader.Services.Abstraction
{
    public interface IClientPaymentService
    {
        Task<IEnumerable<RClientPaymentDto>> GetAllPaymentsAsync();
        Task<IEnumerable<RClientPaymentDto>> GetAllPaymentsByDateAsync(DateOnly date);
        Task<IEnumerable<RClientPaymentDto>> GetAllPaymentsByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<RClientPaymentDto>> GetPaymentsByClientIdAsync(int clientId);
        Task<RClientPaymentDto> GetPaymentByIdAsync(int id);
        Task<RClientPaymentDto> CreatePaymentAsync(WClientPaymentDto payment);
        Task<RClientPaymentDto> UpdatePaymentAsync(int id, WClientPaymentDto payment);
        Task<bool> DeletePaymentAsync(int id); 
    }
}
