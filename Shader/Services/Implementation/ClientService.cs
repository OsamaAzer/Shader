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
            return client.Map<Client, RClientDTO>();
        }
        public async Task<IEnumerable<RAllClientsDTO>> GetAllClientsAsync()
        {
            var clientsDTO =  context.Clients.OrderBy(c => c.Name).Map<Client, RAllClientsDTO>().ToList();
            return await Task.FromResult<IEnumerable<RAllClientsDTO>>(clientsDTO);
        }
        public async Task<RClientDTO> AddClientAsync(WClientDTO dto)
        {
            var client = dto.Map<WClientDTO, Client>();
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
            return client.Map<Client, RClientDTO>();
        }
        public async Task<RClientDTO> UpdateClientAsync(int id, WClientDTO dto)
        {
            var existingClient = await context.Clients.FindAsync(id);
            if (existingClient == null) return null;
            dto.Map(existingClient);
            context.Clients.Update(existingClient);
            await context.SaveChangesAsync();
            return existingClient.Map<Client, RClientDTO>();
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
