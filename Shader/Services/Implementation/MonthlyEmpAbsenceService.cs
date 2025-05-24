using Humanizer;
using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services
{
    public class MonthlyEmpAbsenceService(ShaderContext context) : IMonthlyEmpAbsenceService
    {
        public async Task<IEnumerable<RMonthlyEmpAbsenceDto>> GetAbsencesAsync()
        {
            var absences = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }

        public async Task<IEnumerable<RMonthlyEmpAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId)
        {
            var absences = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }
        public async Task<IEnumerable<RMonthlyEmpAbsenceDto>> GetAbsencesByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            var absences = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.Date >= startDate && a.Date <= endDate && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }
        public async Task<IEnumerable<RMonthlyEmpAbsenceDto>> GetAbsencesForEmployeeByDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate)
        {
            var absences = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && a.Date >= startDate && a.Date <= endDate && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }

        public async Task<RMonthlyEmpAbsenceDto> AddAbsenceAsync(WMonthlyEmpAbsenceDto absenceDto)
        {
            var employee = await context.MonthlyEmployees.FindAsync(absenceDto.EmployeeId) ??
                throw new Exception($"Employee with ID {absenceDto.EmployeeId} not found.");

            


            var absence = absenceDto.MapToAbsence();
            await context.MonthlyEmpAbsences.AddAsync(absence);
            await context.SaveChangesAsync();
            // Reload with employee navigation property
            await context.Entry(absence).Reference(a => a.Employee).LoadAsync();
            return absence.MapToRAbsenceDto();
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
