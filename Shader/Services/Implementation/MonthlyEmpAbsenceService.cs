using Humanizer;
using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services
{
    public class MonthlyEmpAbsenceService(ShaderContext context) : IMonthlyEmpAbsenceService
    {
        public async Task<PagedResponse<RMonthlyEmpAbsenceDto>> GetAbsencesAsync(int pageNumber, int pageSize)
        {
            var absences = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RMonthlyEmpAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize)
        {
            var absences = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RMonthlyEmpAbsenceDto>> GetAbsencesByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var absences = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.Date >= startDate && a.Date <= endDate && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RMonthlyEmpAbsenceDto>> GetAbsencesForEmployeeByDateRangeAsync
            (int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var absences = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && a.Date >= startDate && a.Date <= endDate && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<IEnumerable<RMonthlyEmpAbsenceDto>> AddRangeOfAbsencesAsync(List<int> employeeIds)
        {
            if (employeeIds == null || !employeeIds.Any())
            {
                throw new ArgumentException("Employee IDs cannot be null or empty.", nameof(employeeIds));
            }

            var absences = new List<MonthlyEmployeeAbsence>();
            foreach (var employeeId in employeeIds)
            {
                var employee = await context.MonthlyEmployees.FindAsync(employeeId) ??
                    throw new Exception($"Employee with ID {employeeId} not found.");
                var absenceDto = new WMonthlyEmpAbsenceDto();
                absenceDto.EmployeeId = employeeId;
                var absence = absenceDto.MapToAbsence();
                absence.Employee = employee;
                absences.Add(absence);
            }
            await context.MonthlyEmpAbsences.AddRangeAsync(absences);
            await context.SaveChangesAsync();
            return absences.MapToRAbsenceDtos();
        }

        public async Task<RMonthlyEmpAbsenceDto> UpdateAbsenceAsync(int id, WMonthlyEmpAbsenceDto absenceDto)
        {
            var employee = await context.MonthlyEmployees.FindAsync(absenceDto.EmployeeId) ??
                throw new Exception($"Employee with ID {absenceDto.EmployeeId} not found.");

            var absence = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted)??
                throw new Exception("Absence not found.");

            absence = absenceDto.MapToAbsence(absence);
            context.MonthlyEmpAbsences.Update(absence);
            await context.SaveChangesAsync();
            return absence.MapToRAbsenceDto();
        }

        public async Task<bool> DeleteAbsenceAsync(int id)
        {
            var absence = await context.MonthlyEmpAbsences.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted) ??
                throw new Exception($"Absence with ID {id} not found.");

            absence.IsDeleted = true;
            context.MonthlyEmpAbsences.Update(absence);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
