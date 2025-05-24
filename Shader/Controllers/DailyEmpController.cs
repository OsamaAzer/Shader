using Microsoft.AspNetCore.Mvc;
using Shader.Data.DTOs.DailyEmp;
using Shader.Services.Abstraction;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DailyEmpController(IDailyEmpService dailyEmpService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RDailyEmpDto>>> GetAll()
        {
            var employees = await dailyEmpService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RDailyEmpDto>> GetById(int id)
        {
            try
            {
                var employee = await dailyEmpService.GetEmployeeByIdAsync(id);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<RDailyEmpDto>> Add([FromBody] WDailyEmpDto employeeDto)
        {
            var employee = await dailyEmpService.AddEmployeeAsync(employeeDto);
            return Ok( employee);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<RDailyEmpDto>> Update(int id, [FromBody] WDailyEmpDto employeeDto)
        {
            try
            {
                var updated = await dailyEmpService.UpdateEmployeeAsync(id, employeeDto);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await dailyEmpService.DeleteEmployeeAsync(id);
                if (!result)
                    return StatusCode(500, "Failed to delete employee.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
