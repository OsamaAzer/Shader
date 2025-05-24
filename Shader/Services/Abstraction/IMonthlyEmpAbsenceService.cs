using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IMonthlyEmpAbsenceService
    {
        Task<PagedResponse<RMonthlyEmpAbsenceDto>> GetAbsencesAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RMonthlyEmpAbsenceDto>> GetAbsencesByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<PagedResponse<RMonthlyEmpAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize);
        Task<PagedResponse<RMonthlyEmpAbsenceDto>> GetAbsencesForEmployeeByDateRangeAsync
            (int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<IEnumerable<RMonthlyEmpAbsenceDto>> AddRangeOfAbsencesAsync(List<int> employeeIds);
        Task<RMonthlyEmpAbsenceDto> UpdateAbsenceAsync(int id, WMonthlyEmpAbsenceDto absence);
        Task<bool> DeleteAbsenceAsync(int id);
    }
}
