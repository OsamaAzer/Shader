using Shader.Data.DTOs.DailySalaryRecording;

namespace Shader.Services.Abstraction
{
    public interface IDailySRecordingService
    {
        Task<IEnumerable<RDailySRecordingDto>> GetAllAsync();
        Task<IEnumerable<RDailySRecordingDto>> GetAllByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<RDailySRecordingDto>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<RDailySRecordingDto>> GetByEmployeeIdAndDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate);
        Task<RDailySRecordingDto> GetByIdAsync(int id);
        Task<RDailySRecordingDto> CreateAsync(WDailySRecordingDto dailySRecordingDto);
        Task<RDailySRecordingDto> UpdateAsync(int id, WDailySRecordingDto dailySRecordingDto);
        Task<bool> DeleteAsync(int id);
    }
}
