using Shader.Data.DTOs.ShaderTransaction;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IMerchantTransactionService
    {
        Task<PagedResponse<RMerchantTDto>> GetAllTransactionsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RMerchantTDto>> GetAllTransactionsByDateAsync(DateOnly date, int pageNumber, int pageSize);
        Task<PagedResponse<RMerchantTDto>> GetAllTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<PagedResponse<RMerchantTDto>> GetTransactionsByMerchantIdAsync(int MerchantId, int pageNumber, int pageSize);
        Task<RMerchantTDetailsDto> GetTransactionByIdAsync(int id);
        Task<RMerchantTDetailsDto> CreateTransactionAsync(WMerchantTDto transactionDto);
        Task<RMerchantTDetailsDto> UpdateTransactionAsync(int id, WMerchantTDto transactionDto);
        Task<bool> DeleteTransactionAsync(int id); 
    }
}
