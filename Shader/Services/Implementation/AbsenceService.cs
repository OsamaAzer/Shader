using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.Absence;
using Shader.Data.Entities;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services
{
    public class AbsenceService(ShaderContext context) : IAbsenceService
    {
        public async Task<IEnumerable<RAbsenceDto>> GetAbsencesAsync()
        {
            var absences = await context.Absences
                .Include(a => a.Employee)
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }

        public async Task<IEnumerable<RAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId)
        {
            var absences = await context.Absences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }

        public async Task<IEnumerable<RAbsenceDto>> GetAbsencesForEmployeeByMonthAsync(int employeeId, int month)
        {
            var absences = await context.Absences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && a.Date.Month == month && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos();
        }

        public async Task<RAbsenceDto> AddAbsenceAsync(WAbsenceDto absenceDto)
        {
            var absence = absenceDto.MapToAbsence();
            absence.Date = DateTime.Now;

            context.Absences.Add(absence);
            await context.SaveChangesAsync();

            // Reload with employee navigation property
            await context.Entry(absence).Reference(a => a.Employee).LoadAsync();

            return absence.MapToRAbsenceDto();
        }

        public async Task<RAbsenceDto> UpdateAbsenceAsync(WAbsenceDto absenceDto)
        {
            var absence = await context.Absences
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.EmployeeId == absenceDto.EmployeeId && !a.IsDeleted);

            if (absence == null)
                throw new Exception("Absence not found.");

            absence = absenceDto.MapToAbsence(absence);
            await context.SaveChangesAsync();

            return absence.MapToRAbsenceDto();
        }

        public async Task<bool> DeleteAbsenceAsync(int id)
        {
            var absence = await context.Absences.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            if (absence == null)
                return false;

            absence.IsDeleted = true;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
