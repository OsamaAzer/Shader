using Shader.Data.DTOs.MonthlySalaryRecording;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IMonthlySRecordingService
    {
        Task<PagedResponse<RMonthlySRecordingDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RMonthlySRecordingDto>> GetAllByDateRangeAsync(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<PagedResponse<RMonthlySRecordingDto>> GetAllByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize);
        Task<PagedResponse<RMonthlySRecordingDto>> GetAllByEmployeeIdAndDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<RMonthlySRecordingDto> GetByIdAsync(int id);
        Task<IEnumerable<RMonthlySRecordingDto>> AddAsync(List<int> employees, int month);
        Task<RMonthlySRecordingDto> UpdateAsync(int id, WMonthlySRecordingDto monthlySRecordingDto);
        Task<bool> DeleteAsync(int id);
    }
}
