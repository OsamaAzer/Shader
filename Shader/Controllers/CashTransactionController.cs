using Microsoft.AspNetCore.Mvc;
using Shader.Services.Abstraction;
using Shader.Data.DTOs;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CashTransactionController : ControllerBase
    {
        private readonly ICashTransactionService _cashTransactionService;

        public CashTransactionController(ICashTransactionService cashTransactionService)
        {
            _cashTransactionService = cashTransactionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCashTransaction([FromBody] WCashTransactionDTO cashTransactionDTO)
        {
            if (cashTransactionDTO == null)
                return BadRequest("Invalid data.");

            var result = await _cashTransactionService.AddCashTransactionAsync(cashTransactionDTO);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCashTransaction(int id, [FromBody] WCashTransactionDTO cashTransactionDTO)
        {
            if (cashTransactionDTO == null)
                return BadRequest("Invalid data.");

            var result = await _cashTransactionService.UpdateCashTransactionAsync(id, cashTransactionDTO);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCashTransaction(int id)
        {
            var result = await _cashTransactionService.DeleteCashTransactionAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCashTransactionById(int id)
        {
            var result = await _cashTransactionService.GetCashTransactionByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCashTransactions()
        {
            var result = await _cashTransactionService.GetAllCashTransactionsAsync();
            return Ok(result);
        }

        [HttpGet("by-date-range")]
        public async Task<IActionResult> GetCashTransactionsByDateRange([FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate, [FromQuery] TimeOnly? startTime, [FromQuery] TimeOnly? endTime)
        {
            if ((startDate is null || endDate is null) && (startDate is not null || endDate is not null))
                return BadRequest("Both dates are required!!");
            if ((startTime is null || endTime is null) && (startTime is not null || endTime is not null))
                return BadRequest("Both times are required!!");
            if(startDate >= endDate)
                return BadRequest("StartDate must be less than EndDate!");
            if (startTime >= endTime)
                return BadRequest("StartTime must be less than EndTime!");
            var result = await _cashTransactionService.GetCashTransactionsByDateAndTimeRangeAsync(startDate, endDate, startTime, endTime);
            return Ok(result);
        }
    }
}
