using Shader.Data.DTOs.MonthlyEmp;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class MonthlyEmployeeProfile
    {
        public static IEnumerable<RMonthlyEmpDto> MapToREmployeeDtos(this IEnumerable<MonthlyEmployee> employees)
        {
            return employees.Select(e => new RMonthlyEmpDto
            {
                Id = e.Id,
                Name = e.Name,
                PhoneNumber = e.PhoneNumber,
                Salary = e.Salary
            });
        }
        public static RMonthlyEmpDto MapToREmployeeDto(this MonthlyEmployee employee)
        {
            return new RMonthlyEmpDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                Salary = employee.Salary,
                BorrowedAmount = employee.BorrowedAmount,
            };
        }
        public static MonthlyEmployee MapToEmployee(this WMonthlyEmpDto employeeDto, MonthlyEmployee? employee = null)
        {
            employee ??= new MonthlyEmployee();
            employee.Name = employeeDto.Name;
            employee.Salary = employeeDto.Salary;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            return employee;
        }
    }
}
