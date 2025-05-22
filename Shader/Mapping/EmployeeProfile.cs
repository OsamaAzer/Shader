using Shader.Data.DTOs.Employee;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class EmployeeProfile
    {
        public static IEnumerable<REmployeeDto> MapToREmployeeDtos(this IEnumerable<Employee> employees)
        {
            return employees.Select(e => new REmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                PhoneNumber = e.PhoneNumber,
                SalaryType = e.SalaryType,
                Salary = e.Salary
            });
        }
        public static REmployeeDto MapToREmployeeDto(this Employee employee)
        {
            return new REmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                SalaryType = employee.SalaryType,
                Salary = employee.Salary,
                BorrowedAmount = employee.BorrowedAmount,
            };
        }
        public static Employee MapToEmployee(this WEmployeeDto employeeDto, Employee? employee = null)
        {
            employee ??= new Employee();
            employee.Name = employeeDto.Name;
            employee.SalaryType = employeeDto.SalaryType;
            employee.Salary = employeeDto.Salary;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            return employee;
        }
    }
}
