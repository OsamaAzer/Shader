using Microsoft.AspNetCore.Mvc;
using Shader.Services.Abstraction;
using Shader.Data.Dtos.CashTransaction;
using System.Drawing.Printing;

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

        

        [HttpGet]
        public async Task<IActionResult> GetAllCashTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _cashTransactionService.GetAllCashTransactionsAsync(pageNumber , pageSize );
            return Ok(result);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayCashTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var result = await _cashTransactionService.GetCashTransactionsByDateAsync(today, pageNumber , pageSize );
            return Ok(result);
        }

        [HttpGet("date")]
        public async Task<IActionResult> GetCashTransactionsByDate([FromQuery] DateOnly date, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _cashTransactionService.GetCashTransactionsByDateAsync(date, pageNumber , pageSize );
            return Ok(result);
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetCashTransactionsByDateRange
            ([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _cashTransactionService.GetCashTransactionsByDateRangeAsync(startDate, endDate, pageNumber , pageSize );
                return Ok(result);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCashTransactionById(int id)
        {
            try
            {
                var result = await _cashTransactionService.GetCashTransactionByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddCashTransaction([FromBody] WCashTDto cashTransactionDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _cashTransactionService.AddCashTransactionAsync(cashTransactionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCashTransaction(int id, [FromBody] WCashTDto cashTransactionDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _cashTransactionService.UpdateCashTransactionAsync(id, cashTransactionDto);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }



        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCashTransaction(int id)
        {
            try
            {
                var result = await _cashTransactionService.DeleteCashTransactionAsync(id);
                if (!result) return BadRequest("Something went wrong while deleting transaction!");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        
    }
}
