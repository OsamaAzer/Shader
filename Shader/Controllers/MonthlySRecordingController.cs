using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs.MonthlySalaryRecording;
using Shader.Services.Abstraction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthlySRecordingController(IMonthlySRecordingService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await service.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("by-date-range")]
        public async Task<IActionResult> GetAllByDateRange(
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await service.GetAllByDateRangeAsync(startDate, endDate, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("by-employee/{employeeId}")]
        public async Task<IActionResult> GetAllByEmployeeId(
            int employeeId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await service.GetAllByEmployeeIdAsync(employeeId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("by-employee-date-range")]
        public async Task<IActionResult> GetAllByEmployeeIdAndDateRange(
            [FromQuery] int employeeId,
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await service.GetAllByEmployeeIdAndDateRangeAsync(employeeId, startDate, endDate, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await service.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] List<int> employeeIds)
        {
            try
            {
                var result = await service.AddAsync(employeeIds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WMonthlySRecordingDto dto)
        {
            try
            {
                var result = await service.UpdateAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await service.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
