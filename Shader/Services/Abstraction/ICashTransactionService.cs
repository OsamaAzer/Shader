using Shader.Data.Dtos.CashTransaction;
using Shader.Data.Entities;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface ICashTransactionService
    {
        Task<RCashTDto> AddCashTransactionAsync(WCashTDto cashTransactionDto);
        Task<RCashTDto> UpdateCashTransactionAsync(int id, WCashTDto cashTransactionDto);
        Task<bool> DeleteCashTransactionAsync(int id);
        Task<RCashTDto> GetCashTransactionByIdAsync(int id);
        Task<PagedResponse<RCashTDto>> GetAllCashTransactionsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RCashTDto>> GetCashTransactionsByDateAsync(DateOnly date, int pageNumber, int pageSize);
        Task<PagedResponse<RCashTDto>> GetCashTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
    }
}
