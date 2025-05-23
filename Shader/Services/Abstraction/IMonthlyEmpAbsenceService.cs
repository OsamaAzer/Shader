using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IMonthlyEmpAbsenceService
    {
        Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesAsync();
        Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesForEmployeeByDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate);
        Task<RDEmpAbsenceDto> AddAbsenceAsync(WMEmpAbsenceDto absence);
        Task<RDEmpAbsenceDto> UpdateAbsenceAsync(int id, WMEmpAbsenceDto absence);
        Task<bool> DeleteAbsenceAsync(int id);
    }
}
