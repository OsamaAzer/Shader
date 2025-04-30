using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs;
using Shader.Services.Abstraction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FruitController : ControllerBase
    {
        private readonly IFruitService _fruitService;

        public FruitController(IFruitService fruitService)
        {
            _fruitService = fruitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFruits()
        {
            var fruits = await _fruitService.GetAllFruitsAsync();
            return Ok(fruits);
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<IActionResult> GetAllSupplierFruits(int supplierId)
        {
            var fruits = await _fruitService.GetAllSupplierFruitsAsync(supplierId);
            return Ok(fruits);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFruitById(int id)
        {
            var fruit = await _fruitService.GetFruitByIdAsync(id);
            if (fruit == null) return NotFound();
            return Ok(fruit);
        }

        [HttpPost]
        public async Task<IActionResult> AddFruit([FromBody] AFruitDTO fruitDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var fruit = await _fruitService.AddFruitAsync(fruitDto);
            return Ok(fruit);
        }

        [HttpPost("bulk/{supplierId}")]
        public async Task<IActionResult> AddFruits(int supplierId, [FromBody] List<WRangeFruitDTO> fruitDtos)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var fruits = await _fruitService.AddFruitsAsync(supplierId, fruitDtos);
            return Ok(fruits);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFruit(int id, [FromBody] UFruitDTO fruitDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedFruit = await _fruitService.UpdateFruitAsync(id, fruitDto);
            if (updatedFruit == null) return NotFound();
            return Ok(updatedFruit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFruit(int id)
        {
            var result = await _fruitService.DeleteFruitAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
