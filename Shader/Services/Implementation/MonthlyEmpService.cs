using Shader.Data.DTOs.MonthlyEmp;
using Shader.Data.Entities;
using Shader.Services.Abstraction;
using Shader.Data;
using Microsoft.EntityFrameworkCore;
using Shader.Enums;
using Shader.Mapping;

namespace Shader.Services
{
    public class MonthlyEmpService(ShaderContext context) : IMonthlyEmpService
    {
        public async Task<IEnumerable<RMonthlyEmpDto>> GetAllEmployeesAsync()
        {
            var employees = await context.MonthlyEmployees
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            return employees.MapToREmployeeDtos();
        }

        public async Task<RMonthlyEmpDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted)??
                throw new Exception($"Employee with ID {id} not found."); 

            return employee.MapToREmployeeDto();
        }

        public async Task<RMonthlyEmpDto> AddEmployeeAsync(WMonthlyEmpDto employeeDto)
        {
            var employee = employeeDto.MapToEmployee();
            await context.MonthlyEmployees.AddAsync(employee);
            await context.SaveChangesAsync();
            return employee.MapToREmployeeDto();
        }

        public async Task<RMonthlyEmpDto> UpdateEmployeeAsync(int id, WMonthlyEmpDto employeeDto)
        {
            var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted) ??
                throw new Exception($"Employee with ID {id} not found.");

            employeeDto.MapToEmployee(employee);
            context.MonthlyEmployees.Update(employee);
            await context.SaveChangesAsync();
            return employee.MapToREmployeeDto();
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await context.MonthlyEmployees
                 .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted) ??
             throw new Exception($"Employee with ID {id} not found.");

            employee.IsDeleted = true;
            context.MonthlyEmployees.Update(employee);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
