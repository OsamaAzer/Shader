using Shader.Data.Dtos.Client;
using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.Entities;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IClientTransactionService
    {
        
        Task<PagedResponse<RClientTDto>> GetTransactionsByClientIdAsync(int clientId, int pageNumber, int pageSize); 
        Task<PagedResponse<RClientTDto>> GetTransactionsByDateAsync(DateOnly date, int pageNumber, int pageSize);
        Task<PagedResponse<RClientTDto>> GetAllTransactionsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RClientTDto>> GetTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<RClientTDetailsDto> GetTransactionByIdAsync(int id);
        Task<RClientTDetailsDto> AddTransactionAsync(WClientTDto cashTransactionDto);
        Task<RClientTDetailsDto> UpdateTransactionAsync(int id, WClientTDto cashTransactionDto);
        Task<bool> DeleteTransactionAsync(int id);
        // todo : bool flag to inform if the transaction is paid or not
        // todo : shader interact with another shader
    }
}
