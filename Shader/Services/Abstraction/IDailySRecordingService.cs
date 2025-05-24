using Shader.Data.DTOs.DailySalaryRecording;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IDailySRecordingService
    {
        Task<PagedResponse<RDailySRecordingDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RDailySRecordingDto>> GetAllByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<PagedResponse<RDailySRecordingDto>> GetByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize);
        Task<PagedResponse<RDailySRecordingDto>> GetByEmployeeIdAndDateRangeAsync
            (int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<RDailySRecordingDto> GetByIdAsync(int id);
        Task<IEnumerable<RDailySRecordingDto>> CreateAsync(List<int> employeeIds);
        Task<RDailySRecordingDto> UpdateAsync(int id, WDailySRecordingDto dailySRecordingDto);
        Task<bool> DeleteAsync(int id);
    }
}
