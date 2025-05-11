using Shader.Data.Dtos.CashTransaction;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface ICashTransactionService
    {
        Task<RCashTDto> AddCashTransactionAsync(WCashTDto cashTransactionDto);
        Task<RCashTDto> UpdateCashTransactionAsync(int id, WCashTDto cashTransactionDto);
        Task<bool> DeleteCashTransactionAsync(int id);
        Task<RCashTDto> GetCashTransactionByIdAsync(int id);
        Task<IEnumerable<RCashTDto>> GetAllCashTransactionsAsync();
        Task<IEnumerable<RCashTDto>> GetCashTransactionsByDateAsync(DateOnly date);
        Task<IEnumerable<RCashTDto>> GetCashTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate);
    }
}
