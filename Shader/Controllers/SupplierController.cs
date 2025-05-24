using Microsoft.AspNetCore.Mvc;
using Shader.Data.Dtos.Supplier;
using Shader.Services.Abstraction;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            try
            {
                var suppliers = await _supplierService.GetAllSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("merchant-suppliers")]
        public async Task<IActionResult> GetAllMerchantSuppliers()
        {
            try
            {
                var suppliers = await _supplierService.GetAllMerchantSuppliersAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetSuppliersWithName(string name)
        {
            try
            {
                var suppliers = await _supplierService.GetAllSuppliersWithNameAsync(name);
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            try
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(id);
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] WSupplierDto supplierDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _supplierService.AddSupplierAsync(supplierDto);
                if (result is null) return StatusCode(500, "An error occurred while adding the supplier.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("merchnat-supplier")]
        public async Task<IActionResult> AddMerchantSupplier(int merchantId)
        {
            try
            {
                var result = await _supplierService.AddMerchantAsSupplierAsync(merchantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("merchnat-supplier/{id}")]
        public async Task<IActionResult> UpdateMerchantSupplier(int id, int merchantId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var result = await _supplierService.UpdateMerchantAsSupplierAsync(id, merchantId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] WSupplierDto supplierDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _supplierService.UpdateSupplierAsync(id, supplierDto);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                var result = await _supplierService.DeleteSupplierAsync(id);
                if (!result) return BadRequest("Something went wrong in the delete operation!");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
