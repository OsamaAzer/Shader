using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.DailySalaryRecording;
using Shader.Data.Entities;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class DailySRecordingService(ShaderContext context) : IDailySRecordingService
    {
        public async Task<PagedResponse<RDailySRecordingDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var dailySRecordings = await context.DailyEmpSalaryRecordings
                .Include(d => d.Employee)
                .Where(d => !d.IsDeleted)
                .ToListAsync();

            return dailySRecordings.ToRDailySRecordingDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RDailySRecordingDto>> GetAllByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var dailySRecordings = await context.DailyEmpSalaryRecordings
                .Include(d => d.Employee)
                .Where(d => d.Date >= startDate && d.Date <= endDate && !d.IsDeleted)
                .ToListAsync();

            return dailySRecordings.ToRDailySRecordingDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RDailySRecordingDto>> GetByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize)
        {
            var dailySRecordings = await context.DailyEmpSalaryRecordings
                .Include(d => d.Employee)
                .Where(d => d.EmployeeId == employeeId && !d.IsDeleted)
                .ToListAsync();

            return dailySRecordings.ToRDailySRecordingDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RDailySRecordingDto>> GetByEmployeeIdAndDateRangeAsync
            (int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var dailySRecordings = await context.DailyEmpSalaryRecordings
                .Include(d => d.Employee)
                .Where(d => d.EmployeeId == employeeId && d.Date >= startDate && d.Date <= endDate && !d.IsDeleted)
                .ToListAsync();

            return dailySRecordings.ToRDailySRecordingDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<RDailySRecordingDto> GetByIdAsync(int id)
        {
            var dailySRecording = await context.DailyEmpSalaryRecordings
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted) ??
                throw new Exception($"Daily Salary Recording with ID {id} not found.");

            return dailySRecording.ToRDailySRecordingDto();
        }

        public async Task<IEnumerable<RDailySRecordingDto>> AddRangeAsync(List<int> employeeIds)
        {
            if (employeeIds == null || !employeeIds.Any())
                throw new ArgumentException("Employee IDs cannot be null or empty.", nameof(employeeIds));

            var dailySRecordings = new List<DailyEmpSalaryRecording>();
            foreach (var employeeId in employeeIds)
            {
                var employee = await context.DailyEmployees.FindAsync(employeeId) ??
                    throw new Exception($"Employee with ID {employeeId} not found.");

                var dailySRecordingDto = new WDailySRecordingDto();
                dailySRecordingDto.EmployeeId = employeeId;
                var dailySRecording = dailySRecordingDto.ToDailySRecording();
                //dailySRecording.Employee = employee;
                dailySRecordings.Add(dailySRecording);
            }
            await context.DailyEmpSalaryRecordings.AddRangeAsync(dailySRecordings);
            await context.SaveChangesAsync();
            return dailySRecordings.ToRDailySRecordingDtos();
        }

        public async Task<RDailySRecordingDto> UpdateAsync(int id, WDailySRecordingDto dailySRecordingDto)
        {
            var dailySRecording = await context.DailyEmpSalaryRecordings
                .Include(d => d.Employee)
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted) ??
                throw new Exception($"Daily Salary Recording with ID {id} not found.");

            var employee = await context.DailyEmployees.FindAsync(dailySRecordingDto.EmployeeId) ??
                throw new Exception($"Employee with ID {dailySRecordingDto.EmployeeId} not found.");

            dailySRecording = dailySRecordingDto.ToDailySRecording(dailySRecording);
            context.DailyEmpSalaryRecordings.Update(dailySRecording);
            await context.SaveChangesAsync();
            return dailySRecording.ToRDailySRecordingDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dailySRecording = await context.DailyEmpSalaryRecordings
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted) ??
                throw new Exception($"Daily Salary Recording with ID {id} not found.");

            dailySRecording.IsDeleted = true;
            context.DailyEmpSalaryRecordings.Update(dailySRecording);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
