using Shader.Data.Dtos.Client;
using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.Entities;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IClientTransactionService
    {
        
        Task<PagedResponse<RClientTDto>> GetClientTransactionsByClientIdAsync(int clientId, int pageNumber, int pageSize); 
        Task<PagedResponse<RClientTDto>> GetUnPaidClientTransactionsByClientIdAsync(int clientId, int pageNumber, int pageSize);
        Task<PagedResponse<RClientTDto>> GetClientTransactionsByDateAsync(DateOnly date, int pageNumber, int pageSize);
        Task<PagedResponse<RClientTDto>> GetAllClientTransactionsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RClientTDto>> GetClientTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<RClientTDetailsDto> GetClientTransactionByIdAsync(int id);
        Task<RClientTDetailsDto> AddClientTransactionAsync(WClientTDto cashTransactionDto);
        Task<RClientTDetailsDto> UpdateClientTransactionAsync(int id, WClientTDto cashTransactionDto);
        Task<RClientTDetailsDto> UpdateClientTransactionWithPayments
            (int id, decimal paidAmount, decimal discountAmount, decimal cageMortgageAmountPaid);
        Task<bool> DeleteClientTransactionAsync(int id);
        // todo : bool flag to inform if the transaction is paid or not
        // todo : shader interact with another shader
    }
}
