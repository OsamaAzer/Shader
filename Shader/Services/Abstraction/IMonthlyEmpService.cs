using Shader.Data.DTOs.MonthlyEmp;

namespace Shader.Services.Abstraction
{
    public interface IMonthlyEmpService
    {
        Task<IEnumerable<RMonthlyEmpDto>> GetAllEmployeesAsync();
        Task<RMonthlyEmpDto> GetEmployeeByIdAsync(int id);
        Task<RMonthlyEmpDto> AddEmployeeAsync(WMonthlyEmpDto employee);
        Task<RMonthlyEmpDto> UpdateEmployeeAsync(int id, WMonthlyEmpDto employee);
        Task<bool> DeleteEmployeeAsync(int id);
        
    }
}
