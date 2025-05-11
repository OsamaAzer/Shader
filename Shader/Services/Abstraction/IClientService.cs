using Shader.Data.Dtos.Client;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IClientService
    {
        Task<RClientDto> GetClientByIdAsync(int id);
        Task<IEnumerable<RAllClientsDto>> GetAllClientsAsync();
        Task<IEnumerable<RAllClientsDto>> GetAllClientsWithNameAsync(string name);
        Task<RClientDto> AddClientAsync(WClientDto dto);
        Task<RClientDto> UpdateClientAsync(int id, WClientDto dto);
        Task<RClientDto> UpdateClientTransactionPayments(int clientId, decimal paidAmount, decimal mortgageAmount); 
        Task<Client> UpdateClientAggregatesAsync(Client client);
        Task<bool> DeleteClientAsync(int id);
    }
}
