using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IMonthlyEmpAbsenceService
    {
        Task<IEnumerable<RMonthlyEmpAbsenceDto>> GetAbsencesAsync();
        Task<IEnumerable<RMonthlyEmpAbsenceDto>> GetAbsencesByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<RMonthlyEmpAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<RMonthlyEmpAbsenceDto>> GetAbsencesForEmployeeByDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate);
        Task<RMonthlyEmpAbsenceDto> AddAbsenceAsync(WMonthlyEmpAbsenceDto absence);
        Task<RMonthlyEmpAbsenceDto> UpdateAbsenceAsync(int id, WMonthlyEmpAbsenceDto absence);
        Task<bool> DeleteAbsenceAsync(int id);
    }
}
