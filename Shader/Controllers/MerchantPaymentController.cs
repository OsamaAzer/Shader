using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs.MerchantPayment;
using Shader.Services.Abstraction;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantPaymentController(IMerchantPaymentService merchantPaymentService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RMerchantPaymentDto>>> GetAllPayments()
        {
            var result = await merchantPaymentService.GetAllPaymentsAsync();
            return Ok(result);
        }

        [HttpGet("by-date")]
        public async Task<ActionResult<IEnumerable<RMerchantPaymentDto>>> GetAllPaymentsByDate([FromQuery] DateOnly date)
        {
            var result = await merchantPaymentService.GetAllPaymentsByDateAsync(date);
            return Ok(result);
        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<RMerchantPaymentDto>>> GetAllPaymentsToday()
        {
            var result = await merchantPaymentService.GetAllPaymentsByDateAsync(DateOnly.FromDateTime(DateTime.Today));
            return Ok(result);
        }

        [HttpGet("by-date-range")]
        public async Task<ActionResult<IEnumerable<RMerchantPaymentDto>>> GetAllPaymentsByDateRange([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            var result = await merchantPaymentService.GetAllPaymentsByDateRangeAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("by-merchant/{merchantId:int}")]
        public async Task<ActionResult<IEnumerable<RMerchantPaymentDto>>> GetPaymentsByMerchantId(int merchantId)
        {
            try
            {
                var result = await merchantPaymentService.GetPaymentsByMerchantIdAsync(merchantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RMerchantPaymentDto>> GetPaymentById(int id)
        {
            try
            {
                var result = await merchantPaymentService.GetPaymentByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult<RMerchantPaymentDto>> CreatePayment([FromBody] WMerchantPaymentDto payment)
            {
                try
                {
                    var result = await merchantPaymentService.CreatePaymentAsync(payment);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest($"{ex.Message}");
                }
            }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<RMerchantPaymentDto>> UpdatePayment(int id, [FromBody] WMerchantPaymentDto payment)
        {
            try
            {
                var result = await merchantPaymentService.UpdatePaymentAsync(id, payment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            try
            {
                var deleted = await merchantPaymentService.DeletePaymentAsync(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch(Exception ex)
            {
                return NotFound($"{ex.Message}");
            }

        }
    }
}
