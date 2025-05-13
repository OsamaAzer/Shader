using Microsoft.AspNetCore.Mvc;
using Shader.Services.Abstraction;
using Shader.Data.DTOs.ShaderTransaction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantTransactionController : ControllerBase
    {
        private readonly IMerchantTransactionService _merchantTransactionService;

        public MerchantTransactionController(IMerchantTransactionService merchantTransactionService)
        {
            _merchantTransactionService = merchantTransactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _merchantTransactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetTransactionsByDate([FromRoute] DateOnly date)
        {
            var transactions = await _merchantTransactionService.GetAllTransactionsByDateAsync(date);
            return Ok(transactions);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayTransactions()
        {
            var transactions = await _merchantTransactionService.GetAllTransactionsByDateAsync(DateOnly.FromDateTime(DateTime.Now));
            return Ok(transactions);
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetTransactionsByDateRange([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            try
            {
                var transactions = await _merchantTransactionService.GetAllTransactionsByDateRangeAsync(startDate, endDate);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById([FromRoute] int id)
        {
            try
            {
                var transaction = await _merchantTransactionService.GetTransactionByIdAsync(id);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("merchant/{merchantId}")]
        public async Task<IActionResult> GetTransactionsByMerchantId([FromRoute] int merchantId)
        {
            try
            {
                var transactions = await _merchantTransactionService.GetTransactionsByMerchantIdAsync(merchantId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] WMerchantTDto transactionDto)
        {
            try
            {
                var createdTransaction = await _merchantTransactionService.CreateTransactionAsync(transactionDto);
                return Ok(createdTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction([FromRoute] int id, [FromBody] WMerchantTDto transactionDto)
        {
            try
            {
                var updatedTransaction = await _merchantTransactionService.UpdateTransactionAsync(id, transactionDto);
                return Ok(updatedTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] int id)
        {
            try
            {
                var result = await _merchantTransactionService.DeleteTransactionAsync(id);
                if (result)
                    return NoContent();
                return BadRequest("Failed to delete the transaction.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
