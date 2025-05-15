using Shader.Data.Dtos.Client;
using Shader.Data.Entities;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IClientService
    {
        Task<RClientDto> GetClientByIdAsync(int id);
        Task<PagedResponse<RAllClientsDto>> GetAllClientsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RAllClientsDto>> GetAllClientsWithNameAsync(string name, int pageNumber, int pageSize);
        Task<RClientDto> AddClientAsync(WClientDto dto);
        Task<RClientDto> UpdateClientAsync(int id, WClientDto dto);
        Task<Client> UpdateClientAggregatesAsync(Client client);
        Task<bool> DeleteClientAsync(int id);
    }
}
