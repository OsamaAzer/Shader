using Microsoft.AspNetCore.Mvc;
using Shader.Data.Dtos.Fruit;
using Shader.Data.Entities;
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

        [HttpGet("in-stock")]
        public async Task<IActionResult> GetInStockFruits()
        {
            var fruits = await _fruitService.GetInStockFruitsAsync();
            return Ok(fruits);
        }

        [HttpGet("unavailable")]
        public async Task<IActionResult> GetUnavailableFruits()
        {
            var fruits = await _fruitService.GetUnAvailableFruitsAsync();
            return Ok(fruits);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchFruits([FromQuery] string fruitName)
        {
            var fruits = await _fruitService.SearchWithFruitNameAsync(fruitName);
            return Ok(fruits);
        }

        [HttpGet("supplier-bill/{supplierId}")]
        public async Task<IActionResult> GetSupplierFruitsToBeBilled(int supplierId)
        {
            try
            {
                var fruits = await _fruitService.GetSupplierFruitsToBeBilledAsync(supplierId);
                return Ok(fruits);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("in-stock/supplier/{supplierId}")]
        public async Task<IActionResult> GetInStockSupplierFruits(int supplierId)
        {
            try
            {
                var fruits = await _fruitService.GetInStockSupplierFruitsAsync(supplierId);
                return Ok(fruits);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<IActionResult> GetAllSupplierFruits(int supplierId)
        {
            try
            {
                var fruits = await _fruitService.GetAllSupplierFruitsAsync(supplierId);
                return Ok(fruits);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetFruitById(int id)
        {
            try
            {
                var fruit = await _fruitService.GetFruitByIdAsync(id);
                return Ok(fruit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-cages/{id}")]
        public async Task<IActionResult> AddFruitCages(int id, [FromQuery]int numberOfCages)
        {
            try
            {
                if (numberOfCages <= 0) return BadRequest("Number of cages must be greater than zero.");
                var fruit = await _fruitService.AddFruitCagesAsync(id, numberOfCages);
                if (fruit == null) return BadRequest($"Something went wrong while updating fruit with id: ({id})");
                return Ok(fruit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("bulk/{supplierId}")]
        public async Task<IActionResult> AddFruits(int supplierId, [FromBody] List<WRangeFruitDto> fruitDtos)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var fruits = await _fruitService.AddFruitsAsync(supplierId, fruitDtos);
                if (fruits == null) return BadRequest("Something went wrong while adding fruit!");
                return Ok(fruits);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFruit(int id, [FromBody] UFruitDto fruitDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var updatedFruit = await _fruitService.UpdateFruitAsync(id, fruitDto);
                if (updatedFruit == null) return BadRequest($"Something went wrong while updating fruit with id: ({id})");
                return Ok(updatedFruit);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFruit(int id)
        {
            try
            {
                var result = await _fruitService.DeleteFruitAsync(id);
                if (!result) return BadRequest($"Something went wrong while deleting fruit with id:({id})");
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
