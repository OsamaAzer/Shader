using Shader.Data.DTOs;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IClientTransactionService
    {
        Task<ClientTransaction> AddClientTransactionAsync(WClientTransactionDTO cashTransactionDTO);
        Task<ClientTransaction> UpdateClientTransactionAsync(int id, WClientTransactionDTO cashTransactionDTO);
        Task<bool> DeleteClientTransactionAsync(int id);
        Task<ClientTransaction> GetClientTransactionByIdAsync(int id);
        Task<IEnumerable<ClientTransaction>> GetAllClientTransactionsAsync();
        Task<IEnumerable<ClientTransaction>> GetClientTransactionsByDateAndTimeRangeAsync(DateOnly? startDate, DateOnly? endDate, TimeOnly? startTime, TimeOnly? endTime);

    }
}
