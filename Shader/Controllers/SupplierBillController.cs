using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs.SupplierBill;
using Shader.Services.Abstraction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierBillController : ControllerBase
    {
        private readonly ISupplierBillService _supplierBillService;

        public SupplierBillController(ISupplierBillService supplierBillService)
        {
            _supplierBillService = supplierBillService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSupplierBills([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var bills = await _supplierBillService.GetAllSupplierBillsAsync(pageNumber, pageSize);
                return Ok(bills);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<IActionResult> GetSupplierBillsBySupplierId(int supplierId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var bills = await _supplierBillService.GetSupplierBillsBySupplierIdAsync(supplierId, pageNumber, pageSize);
            return Ok(bills);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierBillById(int id)
        {
            try
            {
                var bill = await _supplierBillService.GetSupplierBillByIdAsync(id);
                return Ok(bill);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSupplierBill([FromBody] WSupplierBillDto supplierBillDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var createdBill = await _supplierBillService.CreateSupplierBillAsync(supplierBillDto);
                return Ok(createdBill);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplierBill(int id, [FromBody] WSupplierBillDto supplierBillDto)
        {
            if (!ModelState.IsValid)return BadRequest(ModelState);
            try
            {
                var updatedBill = await _supplierBillService.UpdateSupplierBillAsync(id, supplierBillDto);
                return Ok(updatedBill);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplierBill(int id)
        {
            try
            {
                var result = await _supplierBillService.DeleteSupplierBillAsync(id);
                if (result) return NoContent();
                return StatusCode(500, "An error occurred while deleting the supplier bill.");
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
