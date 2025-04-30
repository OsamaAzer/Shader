using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs;
using Shader.Data.Entities;
using Shader.Services.Abstraction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientTransactionController : ControllerBase
    {
        private readonly IClientTransactionService _clientTransactionService;

        public ClientTransactionController(IClientTransactionService clientTransactionService)
        {
            _clientTransactionService = clientTransactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClientTransactions()
        {
            var transactions = await _clientTransactionService.GetAllClientTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientTransactionById(int id)
        {
            var transaction = await _clientTransactionService.GetClientTransactionByIdAsync(id);
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetClientTransactionsByDateAndTimeRange(
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate,
            [FromQuery] TimeOnly? startTime,
            [FromQuery] TimeOnly? endTime)
        {
            if ((startDate is null || endDate is null) && (startDate is not null || endDate is not null))
                return BadRequest("Start date and end date are required.");

            if (startDate >= endDate)
                return BadRequest("Start date must be less than end date.");

            if ((startTime is null || endTime is null) && (startTime is not null || endTime is not null))
                return BadRequest("Start time and end time are required.");

            if (startTime >= endTime)
                return BadRequest("Start time must be less than end time."); 

            var transactions = await _clientTransactionService
                .GetClientTransactionsByDateAndTimeRangeAsync(startDate, endDate, startTime, endTime);
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> AddClientTransaction([FromBody] WClientTransactionDTO clientTransactionDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var transaction = await _clientTransactionService.AddClientTransactionAsync(clientTransactionDTO);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClientTransaction(int id, [FromBody] WClientTransactionDTO clientTransactionDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updatedTransaction = await _clientTransactionService.UpdateClientTransactionAsync(id, clientTransactionDTO);
                if (updatedTransaction == null) return NotFound();
                return Ok(updatedTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientTransaction(int id)
        {
            var result = await _clientTransactionService.DeleteClientTransactionAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
