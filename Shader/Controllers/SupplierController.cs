using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs;
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
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplier([FromBody] WSupplierDTO supplierDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _supplierService.AddSupplierAsync(supplierDto);
            if (result is null) return StatusCode(500, "An error occurred while adding the supplier.");
            //return CreatedAtAction(nameof(GetSupplierById), new { id = supplierDto.Id }, supplierDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] WSupplierDTO supplierDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _supplierService.UpdateSupplierAsync(id, supplierDto);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
