using Microsoft.AspNetCore.Mvc;
using Shader.Data.Dtos.ClientTransaction;
using Shader.Data.Entities;
using Shader.Services.Abstraction;
using Shader.Services.Implementation;

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
        public async Task<IActionResult> GetAllClientTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var transactions = await _clientTransactionService.GetAllClientTransactionsAsync(pageNumber, pageSize);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientTransactionById(int id)
        {
            try
            {
                var transaction = await _clientTransactionService.GetClientTransactionByIdAsync(id);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("client/{id}")]
        public async Task<IActionResult> GetClientTransactionsByClientId(int clientId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var transactions = await _clientTransactionService.GetClientTransactionsByClientIdAsync(clientId, pageNumber, pageSize);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("unpaid-client/{id}")]
        public async Task<IActionResult> GetUnPaidClientTransactionsByClientId(int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var transactions = await _clientTransactionService.GetUnPaidClientTransactionsByClientIdAsync(id, pageNumber, pageSize);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("today")]
        public async Task<IActionResult> GetTodayClientTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var transactions = await _clientTransactionService.GetClientTransactionsByDateAsync(today, pageNumber, pageSize);
            return Ok(transactions);
        }
        [HttpGet("date")]
        public async Task<IActionResult> GetClientTransactionsByDate
            ([FromQuery] DateOnly date, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var transactions = await _clientTransactionService.GetClientTransactionsByDateAsync(date, pageNumber, pageSize);
            return Ok(transactions);
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetClientTransactionsByDateAndTimeRange(
            [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var transactions = await _clientTransactionService
                .GetClientTransactionsByDateRangeAsync(startDate, endDate, pageNumber, pageSize);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddClientTransaction([FromBody] WClientTDto clientTransactionDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var transaction = await _clientTransactionService.AddClientTransactionAsync(clientTransactionDto);
                if(transaction == null) return BadRequest("Something went wrong while Adding the transaction!");
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateClientTransaction(int id, [FromBody] WClientTDto clientTransactionDto)
        {

            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var updatedTransaction = await _clientTransactionService.UpdateClientTransactionAsync(id, clientTransactionDto);
                if (updatedTransaction == null) return BadRequest("Something went wrong while updating the transaction!");
                return Ok(updatedTransaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //[HttpPut("payment/{id}")]
        //public async Task<IActionResult> UpdateTransactionWithAmountPaidAndDiscountAmount
        //    (int id , decimal paidAmount, decimal discountAmount, decimal cageMortgageAmountPaid)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    try
        //    {
        //        var updatedTransaction = await _clientTransactionService
        //            .UpdateClientTransactionWithPayments(id, paidAmount, discountAmount, cageMortgageAmountPaid);
        //        if (updatedTransaction == null) return BadRequest("Something went wrong while updating the transaction!");
        //        return Ok(updatedTransaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

       

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientTransaction(int id)
        {
            try
            {
                var result = await _clientTransactionService.DeleteClientTransactionAsync(id);
                if (!result) return BadRequest("Something went wrong while deleting the transaction!");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }
    }
}
