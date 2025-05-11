using Shader.Data.Dtos.Client;
using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IClientTransactionService
    {
        
        Task<IEnumerable<RClientTDto>> GetClientTransactionsByClientIdAsync(int clientId); 
        Task<IEnumerable<RClientTDto>> GetUnPaidClientTransactionsByClientIdAsync(int clientId);
        Task<IEnumerable<RClientTDto>> GetClientTransactionsByDateAsync(DateOnly date);
        Task<IEnumerable<RClientTDto>> GetAllClientTransactionsAsync();
        Task<IEnumerable<RClientTDto>> GetClientTransactionsByDateRangeAsync
            (DateOnly startDate, DateOnly endDate);
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
