using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs.ShaderSeller;
using Shader.Services.Abstraction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMerchants([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var merchants = await _merchantService.GetAllMerchantsAsync(pageNumber, pageSize);
            return Ok(merchants);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetMerchantsByName([FromQuery] string name, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var merchants = await _merchantService.GetAllMerchantsWithNameAsync(name, pageNumber, pageSize);
            return Ok(merchants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMerchantById(int id)
        {
            try
            {
                var merchant = await _merchantService.GetMerchantByIdAsync(id);
                return Ok(merchant);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMerchant([FromBody] WMerchantDto merchantDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var createdMerchant = await _merchantService.CreateMerchantAsync(merchantDto);
                return Ok(createdMerchant);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMerchant(int id, [FromBody] WMerchantDto merchantDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updatedMerchant = await _merchantService.UpdateMerchantAsync(id, merchantDto);
                return Ok(updatedMerchant);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMerchant(int id)
        {
            try
            {
                var result = await _merchantService.DeleteMerchantAsync(id);
                return Ok(new { Success = result });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
