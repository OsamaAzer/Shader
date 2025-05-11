using Shader.Data.DTOs.ShaderTransaction;

namespace Shader.Services.Abstraction
{
    public interface IMerchantTransactionService
    {
        Task<IEnumerable<RMerchantTDto>> GetAllTransactionsAsync();
        Task<IEnumerable<RMerchantTDto>> GetAllTransactionsByDateAsync(DateOnly date);
        Task<IEnumerable<RMerchantTDto>> GetAllTransactionsByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<RMerchantTDetailsDto> GetTransactionByIdAsync(int id);
        Task<IEnumerable<RMerchantTDto>> GetTransactionsByShaderIdAsync(int shaderId);
        Task<RMerchantTDetailsDto> CreateTransactionAsync(WMerchantTDto transactionDto);
        Task<RMerchantTDetailsDto> UpdateTransactionAsync(int id, WMerchantTDto transactionDto);
        Task<bool> DeleteTransactionAsync(int id); 
    }
}
