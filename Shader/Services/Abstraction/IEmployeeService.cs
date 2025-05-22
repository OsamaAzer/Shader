using Shader.Data.DTOs.Employee;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IEmployeeService
    {
        Task<IEnumerable<REmployeeDto>> GetAllEmployeesAsync();
        Task<IEnumerable<REmployeeDto>> GetDailySalaryEmployeesAsync();
        Task<IEnumerable<REmployeeDto>> GetMonthlySalaryEmployeesAsync();
        Task<REmployeeDto> GetEmployeeByIdAsync(int id);
        Task<REmployeeDto> AddEmployeeAsync(WEmployeeDto employee);
        Task<REmployeeDto> UpdateEmployeeAsync(int id, WEmployeeDto employee);
        Task<bool> DeleteEmployeeAsync(int id);
        
    }
}
