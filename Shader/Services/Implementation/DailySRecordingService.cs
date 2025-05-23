using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.DailySalaryRecording;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class DailySRecordingService(ShaderContext context) : IDailySRecordingService
    {
        public async Task<IEnumerable<RDailySRecordingDto>> GetAllAsync()
        {
            var dailySRecordings = await context.DailySalaryRecordings
                .Include(d => d.Employee)
                .Where(d => !d.IsDeleted)
                .ToListAsync();

            return dailySRecordings.ToRDailySRecordingDtos();
        }
        public async Task<IEnumerable<RDailySRecordingDto>> GetAllByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            var dailySRecordings = await context.DailySalaryRecordings
                .Include(d => d.Employee)
                .Where(d => DateOnly.FromDateTime(d.Date) >= startDate && DateOnly.FromDateTime(d.Date) <= endDate && !d.IsDeleted)
                .ToListAsync();

            return dailySRecordings.ToRDailySRecordingDtos();
        }
        public async Task<IEnumerable<RDailySRecordingDto>> GetByEmployeeIdAsync(int employeeId)
        {
            var dailySRecordings = await context.DailySalaryRecordings
                .Include(d => d.Employee)
                .Where(d => d.EmployeeId == employeeId && !d.IsDeleted)
                .ToListAsync();

            return dailySRecordings.ToRDailySRecordingDtos();
        }
        public async Task<IEnumerable<RDailySRecordingDto>> GetByEmployeeIdAndDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate)
        {
            var dailySRecordings = await context.DailySalaryRecordings
                .Include(d => d.Employee)
                .Where(d => d.EmployeeId == employeeId && DateOnly.FromDateTime(d.Date) >= startDate && DateOnly.FromDateTime(d.Date) <= endDate && !d.IsDeleted)
                .ToListAsync();

            return dailySRecordings.ToRDailySRecordingDtos();
        }
        public async Task<RDailySRecordingDto> GetByIdAsync(int id)
        {
            var dailySRecording = await context.DailySalaryRecordings
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted) ??
                throw new Exception($"Daily Salary Recording with ID {id} not found.");

            return dailySRecording.ToRDailySRecordingDto();
        }
        public async Task<RDailySRecordingDto> CreateAsync(WDailySRecordingDto dailySRecordingDto)
        {
            var dailySRecording = dailySRecordingDto.ToDailySRecording();
            dailySRecording.Date = DateTime.Now;
            await context.DailySalaryRecordings.AddAsync(dailySRecording);
            await context.SaveChangesAsync();
            return dailySRecording.ToRDailySRecordingDto();
        }
        public async Task<RDailySRecordingDto> UpdateAsync(int id, WDailySRecordingDto dailySRecordingDto)
        {
            var dailySRecording = await context.DailySalaryRecordings
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted) ??
                throw new Exception($"Daily Salary Recording with ID {id} not found.");

            dailySRecording = dailySRecordingDto.ToDailySRecording(dailySRecording);
            context.DailySalaryRecordings.Update(dailySRecording);
            await context.SaveChangesAsync();
            return dailySRecording.ToRDailySRecordingDto();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var dailySRecording = await context.DailySalaryRecordings
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted) ??
                throw new Exception($"Daily Salary Recording with ID {id} not found.");

            dailySRecording.IsDeleted = true;
            context.DailySalaryRecordings.Update(dailySRecording);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
