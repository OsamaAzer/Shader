using Shader.Data.DTOs.DailyEmpAbsence;
using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Helpers;
using Shader.Services.Implementation;

namespace Shader.Services.Abstraction
{
    public interface IDailyEmpAbsenceService
    {
        Task<PagedResponse<RDailyEmpAbsenceDto>> GetAbsencesAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RDailyEmpAbsenceDto>> GetAbsencesByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<PagedResponse<RDailyEmpAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize);
        Task<PagedResponse<RDailyEmpAbsenceDto>> GetAbsencesForEmployeeByDateRangeAsync
            (int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<IEnumerable<RDailyEmpAbsenceDto>> AddRangeAsync(List<int> employeeIds);
        Task<RDailyEmpAbsenceDto> AddPastAbsenceAsync(WDailyPastAbsenceDto absence);
        Task<RDailyEmpAbsenceDto> UpdateAbsenceAsync(int id, WDailyEmpAbsenceDto absence);
        Task<bool> DeleteAbsenceAsync(int id);
    }
}
