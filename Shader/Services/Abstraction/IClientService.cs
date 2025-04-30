using Shader.Data.DTOs;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IClientService
    {
        Task<RClientDTO> GetClientByIdAsync(int id);
        Task<IEnumerable<RAllClientsDTO>> GetAllClientsAsync();
        Task<RClientDTO> AddClientAsync(WClientDTO dto);
        Task<RClientDTO> UpdateClientAsync(int id, WClientDTO dto);
        Task<bool> DeleteClientAsync(int id);
    }
}
