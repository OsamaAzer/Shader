using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs.ClientPayment;
using Shader.Services.Abstraction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientPaymentController : ControllerBase
    {
        private readonly IClientPaymentService _clientPaymentService;

        public ClientPaymentController(IClientPaymentService clientPaymentService)
        {
            _clientPaymentService = clientPaymentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _clientPaymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetPaymentsByDate([FromRoute] DateOnly date)
        {
            var payments = await _clientPaymentService.GetAllPaymentsByDateAsync(date);
            return Ok(payments);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayPayments()
        {
            var payments = await _clientPaymentService.GetAllPaymentsByDateAsync(DateOnly.FromDateTime(DateTime.Now));
            return Ok(payments);
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetPaymentsByDateRange([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            var payments = await _clientPaymentService.GetAllPaymentsByDateRangeAsync(startDate, endDate);
            return Ok(payments);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetPaymentsByClientId([FromRoute] int clientId)
        {
            try
            {
                var payments = await _clientPaymentService.GetPaymentsByClientIdAsync(clientId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById([FromRoute] int id)
        {
            try
            {
                var payment = await _clientPaymentService.GetPaymentByIdAsync(id);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] WClientPaymentDto payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var createdPayment = await _clientPaymentService.CreatePaymentAsync(payment);
                return Ok(createdPayment);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment([FromRoute] int id, [FromBody] WClientPaymentDto payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var updatedPayment = await _clientPaymentService.UpdatePaymentAsync(id, payment);
                return Ok(updatedPayment);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment([FromRoute] int id)
        {
            try
            {
                var result = await _clientPaymentService.DeletePaymentAsync(id);
                return result ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }   
        }
    }
}
