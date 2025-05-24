using Microsoft.AspNetCore.Mvc;
using Shader.Services.Abstraction;
using Shader.Data.DTOs.MonthlyEmp;

namespace Shader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonthlyEmpController(IMonthlyEmpService monthlyEmpService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RMonthlyEmpDto>>> GetAll()
        {
            try
            {
                var employees = await monthlyEmpService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RMonthlyEmpDto>> GetById(int id)
        {
            try
            {
                var employee = await monthlyEmpService.GetEmployeeByIdAsync(id);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<RMonthlyEmpDto>> Add([FromBody] WMonthlyEmpDto employeeDto)
        {
            try
            {
                var employee = await monthlyEmpService.AddEmployeeAsync(employeeDto);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<RMonthlyEmpDto>> Update(int id, [FromBody] WMonthlyEmpDto employeeDto)
        {
            try
            {
                var employee = await monthlyEmpService.UpdateEmployeeAsync(id, employeeDto);
                return Ok(employee);
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
                var result = await monthlyEmpService.DeleteEmployeeAsync(id);
                if (!result)
                    return NotFound($"Employee with ID {id} not found or already deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
