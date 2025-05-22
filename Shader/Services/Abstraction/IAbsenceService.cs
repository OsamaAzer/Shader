using Shader.Data.DTOs.Absence;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface IAbsenceService
    {
        Task<IEnumerable<RAbsenceDto>> GetAbsencesAsync();
        Task<IEnumerable<RAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<RAbsenceDto>> GetAbsencesForEmployeeByMonthAsync(int employeeId, int month);
        Task<RAbsenceDto> AddAbsenceAsync(WAbsenceDto absence);
        Task<RAbsenceDto> UpdateAbsenceAsync(WAbsenceDto absence);
        Task<bool> DeleteAbsenceAsync(int id);
    }
}
