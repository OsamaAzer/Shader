using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IClientService
    {
        Task<Client> GetClientByIdAsync(int id);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task AddClientAsync(Client client);
        Task UpdateClientAsync(Client client);
        Task DeleteClientAsync(int id);
    }
}
