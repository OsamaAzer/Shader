using Shader.Data.DTOs;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface ICashTransactionService
    {
        Task<CashTransaction> AddCashTransactionAsync(WCashTransactionDTO cashTransactionDTO);
        Task<CashTransaction> UpdateCashTransactionAsync(int id, WCashTransactionDTO cashTransactionDTO);
        Task<bool> DeleteCashTransactionAsync(int id);
        Task<CashTransaction> GetCashTransactionByIdAsync(int id);
        Task<IEnumerable<CashTransaction>> GetAllCashTransactionsAsync();
        Task<IEnumerable<CashTransaction>> GetCashTransactionsByDateAndTimeRangeAsync(DateOnly? startDate, DateOnly? endDate, TimeOnly? startTime, TimeOnly? endTime);
    }
}
