using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs.DailyEmpAbsence;
using Shader.Services.Abstraction;
using Shader.Services.Implementation;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DailyEmpAbsenceController : ControllerBase
    {
        private readonly IDailyEmpAbsenceService _service;

        public DailyEmpAbsenceController(IDailyEmpAbsenceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAbsences([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAbsencesAsync(pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetAbsencesByEmployeeId(int employeeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAbsencesByEmployeeIdAsync(employeeId, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetAbsencesByDateRange([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAbsencesByDateRangeAsync(startDate, endDate, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("employee/{employeeId}/range")]
        public async Task<IActionResult> GetAbsencesForEmployeeByDateRange(int employeeId, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAbsencesForEmployeeByDateRangeAsync(employeeId, startDate, endDate, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("past")]
        public async Task<IActionResult> AddPastAbsence([FromBody] WDailyPastAbsenceDto absenceDto)
        {
            try
            {
                var result = await _service.AddPastAbsenceAsync(absenceDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAbsence([FromBody] List<int> employeeIds)
        {
            try
            {
                var result = await _service.AddRangeAsync(employeeIds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbsence(int id, [FromBody] WDailyEmpAbsenceDto absenceDto)
        {
            try
            {
                var result = await _service.UpdateAbsenceAsync(id, absenceDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbsence(int id)
        {
            try
            {
                var result = await _service.DeleteAbsenceAsync(id);
                if (result)
                    return NoContent();
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
