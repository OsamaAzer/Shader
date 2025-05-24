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
                BaseSalary = e.BaseSalary
            });
        }
        public static RMonthlyEmpDto MapToREmployeeDto(this MonthlyEmployee employee)
        {
            return new RMonthlyEmpDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                BaseSalary = employee.BaseSalary,
                BorrowedAmount = employee.BorrowedAmount,
                RemainingSalary = employee.RemainingSalary
            };
        }
        public static MonthlyEmployee MapToEmployee(this WMonthlyEmpDto employeeDto, MonthlyEmployee? employee = null)
        {
            employee ??= new MonthlyEmployee();
            employee.Name = employeeDto.Name;
            if (employeeDto.BaseSalary <= 0)
            {
                throw new ArgumentException("Base salary must be greater than zero.", nameof(employeeDto.BaseSalary));
            }
            else
            {
                employee.BaseSalary = employeeDto.BaseSalary;
            }
            employee.PhoneNumber = employeeDto.PhoneNumber;
            return employee;
        }
    }
}
