using Shader.Data.DTOs.DailyEmp;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class DailyEmpProfile
    {
        public static RDailyEmpDto MapToREmployeeDto(this DailyEmployee employee)
        {
            return new RDailyEmpDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                DailySalary = employee.DailySalary,
            };
        }
        public static IEnumerable<RDailyEmpDto> MapToREmployeeDtos(this IEnumerable<DailyEmployee> employees)
        {
            return employees.Select(e => e.MapToREmployeeDto());
        }
        public static DailyEmployee MapToEmployee(this WDailyEmpDto employeeDto, DailyEmployee? employee = null)
        {
            employee ??= new DailyEmployee();
            employee.Name = employeeDto.Name;
            employee.DailySalary = employeeDto.DailySalary;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            return employee;
        }
    }
}
