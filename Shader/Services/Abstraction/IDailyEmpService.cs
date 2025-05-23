using Shader.Data.DTOs.DailyEmp;

namespace Shader.Services.Abstraction
{
    public interface IDailyEmpService
    {
        Task<IEnumerable<RDailyEmpDto>> GetAllEmployeesAsync();
        Task<RDailyEmpDto> GetEmployeeByIdAsync(int id);
        Task<RDailyEmpDto> AddEmployeeAsync(WDailyEmpDto employee);
        Task<RDailyEmpDto> UpdateEmployeeAsync(int id, WDailyEmpDto employee);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
