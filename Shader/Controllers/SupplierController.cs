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
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
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

        [HttpGet("name")]
        public async Task<IActionResult> GetSuppliersWithName(string name)
        {
            var suppliers = await _supplierService.GetAllSuppliersWithNameAsync(name);
            return Ok(suppliers);
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] WSupplierDto supplierDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _supplierService.AddSupplierAsync(supplierDto);
            if (result is null) return StatusCode(500, "An error occurred while adding the supplier.");
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] WSupplierDto supplierDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
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
