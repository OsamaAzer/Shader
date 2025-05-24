using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Services.Abstraction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthlyEmpAbsenceController(IMonthlyEmpAbsenceService absenceService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAbsences([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await absenceService.GetAbsencesAsync(pageNumber, pageSize);
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
                var result = await absenceService.GetAbsencesByEmployeeIdAsync(employeeId, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetAbsencesByDateRange([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await absenceService.GetAbsencesByDateRangeAsync(startDate, endDate, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("employee/{employeeId}/date-range")]
        public async Task<IActionResult> GetAbsencesForEmployeeByDateRange(int employeeId, [FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await absenceService.GetAbsencesForEmployeeByDateRangeAsync(employeeId, startDate, endDate, pageNumber, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-range")]
        public async Task<IActionResult> AddRangeOfAbsences([FromBody] List<int> employeeIds)
        {
            try
            {
                var result = await absenceService.AddRangeOfAbsencesAsync(employeeIds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAbsence(int id, [FromBody] WMonthlyEmpAbsenceDto absenceDto)
        {
            try
            {
                var result = await absenceService.UpdateAbsenceAsync(id, absenceDto);
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
                var result = await absenceService.DeleteAbsenceAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
