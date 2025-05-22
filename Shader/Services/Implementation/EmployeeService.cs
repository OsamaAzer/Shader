using Shader.Data.DTOs.Employee;
using Shader.Data.Entities;
using Shader.Services.Abstraction;
using Shader.Data;
using Microsoft.EntityFrameworkCore;
using Shader.Enums;
using Shader.Mapping;

namespace Shader.Services
{
    public class EmployeeService(ShaderContext context) : IEmployeeService
    {
        public async Task<IEnumerable<REmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await context.Employees
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            return employees.MapToREmployeeDtos();
        }

        public async Task<IEnumerable<REmployeeDto>> GetDailySalaryEmployeesAsync()
        {
            var employees = await context.Employees
                .Where(e => e.SalaryType == SalaryType.Daily && !e.IsDeleted)
                .ToListAsync();

            return employees.MapToREmployeeDtos();
        }

        public async Task<IEnumerable<REmployeeDto>> GetMonthlySalaryEmployeesAsync()
        {
            var employees = await context.Employees
                .Where(e => e.SalaryType == SalaryType.Monthly && !e.IsDeleted)
                .ToListAsync();

            return employees.MapToREmployeeDtos();
        }

        public async Task<REmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted)??
                throw new Exception($"Employee with ID {id} not found."); 

            return employee.MapToREmployeeDto();
        }

        public async Task<REmployeeDto> AddEmployeeAsync(WEmployeeDto employeeDto)
        {
            var employee = employeeDto.MapToEmployee();
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return employee.MapToREmployeeDto();
        }

        public async Task<REmployeeDto> UpdateEmployeeAsync(int id, WEmployeeDto employeeDto)
        {
            var employee = await context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted) ??
                throw new Exception($"Employee with ID {id} not found.");

            employeeDto.Map(employee);
            context.Employees.Update(employee);
            await context.SaveChangesAsync();
            return employee.MapToREmployeeDto();
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await context.Employees
                 .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted) ??
             throw new Exception($"Employee with ID {id} not found.");
            employee.IsDeleted = true;
            context.Employees.Update(employee);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
