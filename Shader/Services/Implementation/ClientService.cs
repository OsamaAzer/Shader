using Shader.Data;
using Shader.Data.DTOs;
using Shader.Data.Entities;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class ClientService(ShaderContext context) : IClientService
    {
        public async Task<RClientDTO> GetClientByIdAsync(int id)
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null) return null;
            return client.ToDTO<Client, RClientDTO>();
        }
        public async Task<IEnumerable<RAllClientsDTO>> GetAllClientsAsync()
        {
            var clientsDTO =  context.Clients.OrderBy(c => c.Name).ToDTO<Client, RAllClientsDTO>().ToList();
            return await Task.FromResult<IEnumerable<RAllClientsDTO>>(clientsDTO);
        }
        public async Task<bool> AddClientAsync(WClientDTO dto)
        {
            var client = dto.ToEntity<Client, WClientDTO>();
            await context.Clients.AddAsync(client);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateClientAsync(int id, WClientDTO dto)
        {
            var existingClient = await context.Clients.FindAsync(id);
            if (existingClient == null) return false;
            dto.ToEntity(existingClient);
            context.Clients.Update(existingClient);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteClientAsync(int id)
        {
            var existingClient = await context.Clients.FindAsync(id);
            if (existingClient == null) return false;
            context.Clients.Remove(existingClient);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
