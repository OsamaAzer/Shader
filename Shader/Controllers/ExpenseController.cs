using Microsoft.AspNetCore.Mvc;
using Shader.Data.Dtos.Expense;
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

        [HttpGet("range")]
        public async Task<IActionResult> GetExpensesByDateRange
            ([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var expenses = await _expenseService.GetExpensesByDateAndTimeRangeAsync(startDate, endDate, pageNumber, pageSize);
                return Ok(expenses);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetExpensesByDate([FromQuery] DateOnly date, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var expenses = await _expenseService.GetExpensesByDateAsync(date, pageNumber, pageSize);
            return Ok(expenses);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayExpenses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var expenses = await _expenseService.GetExpensesByDateAsync(today, pageNumber, pageSize);
            return Ok(expenses);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var expenses = await _expenseService.GetAllExpensesAsync(pageNumber, pageSize);
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            try
            {
                var expense = await _expenseService.GetExpenseByIdAsync(id);
                return Ok(expense);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] WExpenseDto expenseDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _expenseService.AddExpenseAsync(expenseDto);
                if (result is null) return StatusCode(500, "An error occurred while adding the expense.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] WExpenseDto expenseDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _expenseService.UpdateExpenseAsync(id, expenseDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                var result = await _expenseService.DeleteExpenseAsync(id);
                if (!result) return BadRequest("Something went wrong while deleting expense!");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
