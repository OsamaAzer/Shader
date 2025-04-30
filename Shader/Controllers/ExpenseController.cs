using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs;
using Shader.Services.Abstraction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("search-with-range")]
        public async Task<IActionResult> GetExpensesByDateRange
            ([FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate, [FromQuery] TimeOnly? startTime, [FromQuery] TimeOnly? endTime)
        {
            if ((startDate is null || endDate is null) && (startDate is not null || endDate is not null))
                return BadRequest("Start date and end date are required.");

            if(startDate >= endDate)
                return BadRequest("Start date must be less than end date.");

            if ((startTime is null || endTime is null) && (startTime is not null || endTime is not null))
                return BadRequest("Start time and end time are required.");

            if (startTime >= endTime)
                return BadRequest("Start time must be less than end time.");

            var expenses = await _expenseService.GetExpensesByDateAndTimeRangeAsync(startDate, endDate, startTime, endTime);
            return Ok(expenses);
        }

        [HttpGet("get-specific-date")]
        public async Task<IActionResult> GetExpensesByDate([FromQuery] DateOnly date)
        {
            var expenses = await _expenseService.GetExpensesByDateAsync(date);
            return Ok(expenses);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null) return NotFound();
            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] WExpenseDTO expenseDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (expenseDto.Amount <= 0) return BadRequest("Amount must be greater than zero.");
            var result = await _expenseService.AddExpenseAsync(expenseDto);
            if (result is null) return StatusCode(500, "An error occurred while adding the expense.");
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] WExpenseDTO expenseDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (expenseDto.Amount <= 0) return BadRequest("Amount must be greater than zero.");
            var result = await _expenseService.UpdateExpenseAsync(id, expenseDto);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var result = await _expenseService.DeleteExpenseAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
