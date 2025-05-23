using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class DailyEmpAbsenceService(ShaderContext context) : IDailyEmpAbsenceService
    {
        public async Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesAsync()
        {
            var absences = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }
        public async Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId)
        {
            var absences = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }
        public async Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            var absences = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => DateOnly.FromDateTime(a.Date) >= startDate && DateOnly.FromDateTime(a.Date) <= endDate && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }
        public async Task<IEnumerable<RDEmpAbsenceDto>> GetAbsencesForEmployeeByDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate)
        {
            var absences = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && DateOnly.FromDateTime(a.Date) >= startDate && DateOnly.FromDateTime(a.Date) <= endDate && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }
        public async Task<RDEmpAbsenceDto> AddAbsenceAsync(WDEmpAbsenceDto absenceDto)
        {
            var absence = absenceDto.MapToAbsence();
            absence.Date = DateTime.Now;
            await context.DailyEmpAbsences.AddAsync(absence);
            await context.SaveChangesAsync();
            // Reload with employee navigation property
            await context.Entry(absence).Reference(a => a.Employee).LoadAsync();
            return absence.MapToRAbsenceDto();
        }
        public async Task<RDEmpAbsenceDto> UpdateAbsenceAsync(int id, WDEmpAbsenceDto absenceDto)
        {
            var absence = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted) ??
                throw new Exception("Absence not found.");

            absenceDto.MapToAbsence(absence);
            context.DailyEmpAbsences.Update(absence);
            await context.SaveChangesAsync();
            return absence.MapToRAbsenceDto();
        }
        public async Task<bool> DeleteAbsenceAsync(int id)
        {
            var absence = await context.DailyEmpAbsences
                 .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted) ??
                 throw new Exception($"Absence with ID {id} not found.");

            absence.IsDeleted = true;
            context.DailyEmpAbsences.Update(absence);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
