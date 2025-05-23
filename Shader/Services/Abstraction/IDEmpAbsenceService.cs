using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Services.Implementation;

namespace Shader.Services.Abstraction
{
    public interface IDailyEmpAbsenceService
    {
        Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesAsync();
        Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesForEmployeeByDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate);
        Task<RDEmpAbsenceDto> AddAbsenceAsync(WDEmpAbsenceDto absence);
        Task<RDEmpAbsenceDto> UpdateAbsenceAsync(int id, WDEmpAbsenceDto absence);
        Task<bool> DeleteAbsenceAsync(int id);
    }
}
