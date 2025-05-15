using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Shader.Data.Dtos.Client;
using Shader.Services.Abstraction;
using Shader.Services.Implementation;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            try
            {
                var client = await _clientService.GetClientByIdAsync(id);
                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetClientsWithName(string name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var clients = await _clientService.GetAllClientsWithNameAsync(name, pageNumber, pageSize);
            return Ok(clients);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var clients = await _clientService.GetAllClientsAsync(pageNumber, pageSize);
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] WClientDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _clientService.AddClientAsync(dto);
                if (result is null) return StatusCode(500, "An error occurred while adding the client.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] WClientDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _clientService.UpdateClientAsync(id, dto);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                var result = await _clientService.DeleteClientAsync(id);
                if (!result) return BadRequest("Something went wrong in the delete operation!");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
