using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.DailyEmp;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class DailyEmpService(ShaderContext context) : IDailyEmpService
    {
        public async Task<IEnumerable<RDailyEmpDto>> GetAllEmployeesAsync()
        {
            var employees = await context.DailyEmployees
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            return employees.MapToREmployeeDtos();
        }
        public async Task<RDailyEmpDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await context.DailyEmployees
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted)??
                throw new Exception($"Employee with ID {id} not found."); 

            return employee.MapToREmployeeDto();
        }
        public async Task<RDailyEmpDto> AddEmployeeAsync(WDailyEmpDto employeeDto)
        {
            var employee = employeeDto.MapToEmployee();
            await context.DailyEmployees.AddAsync(employee);
            await context.SaveChangesAsync();
            return employee.MapToREmployeeDto();
        }
        public async Task<RDailyEmpDto> UpdateEmployeeAsync(int id, WDailyEmpDto employeeDto)
        {
            var employee = await context.DailyEmployees
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted) ??
                throw new Exception($"Employee with ID {id} not found.");

            employeeDto.MapToEmployee(employee);
            context.DailyEmployees.Update(employee);
            await context.SaveChangesAsync();
            return employee.MapToREmployeeDto();
        }
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await context.DailyEmployees
                 .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted) ??
             throw new Exception($"Employee with ID {id} not found.");

            employee.IsDeleted = true;
            context.DailyEmployees.Update(employee);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
