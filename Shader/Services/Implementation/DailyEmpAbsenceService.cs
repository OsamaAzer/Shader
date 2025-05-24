using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.DailyEmpAbsence;
using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class DailyEmpAbsenceService(ShaderContext context) : IDailyEmpAbsenceService
    {
        public async Task<PagedResponse<RDailyEmpAbsenceDto>> GetAbsencesAsync(int pageNumber, int pageSize)
        {
            var absences = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RDailyEmpAbsenceDto>> GetAbsencesByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize)
        {
            var absences = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RDailyEmpAbsenceDto>> GetAbsencesByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date."); 

            var absences = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.Date >= startDate && a.Date <= endDate && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RDailyEmpAbsenceDto>> GetAbsencesForEmployeeByDateRangeAsync
            (int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var absences = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && a.Date >= startDate && a.Date <= endDate && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<RDailyEmpAbsenceDto> AddPastAbsenceAsync(WDailyPastAbsenceDto absenceDto)
        {
            var employee = await context.DailyEmployees
                .FindAsync(absenceDto.EmployeeId) ??
                throw new Exception($"Employee with ID {absenceDto.EmployeeId} not found.");

            var absence = absenceDto.MapFromPastToDailyEmpAbsence();
            await context.DailyEmpAbsences.AddAsync(absence);
            await context.SaveChangesAsync();
            // Reload with employee navigation property
            await context.Entry(absence).Reference(a => a.Employee).LoadAsync();
            return absence.MapToRAbsenceDto();
        }

        public async Task<IEnumerable<RDailyEmpAbsenceDto>> AddRangeAsync(List<int> employeeIds)
        {
            if (employeeIds == null || !employeeIds.Any())
                throw new ArgumentException("Employee IDs cannot be null or empty.");

            var dailyEmpAbsences = new List<DailyEmpAbsence>();
            foreach (var employeeId in employeeIds)
            {
                var employee = await context.DailyEmployees
                    .FirstOrDefaultAsync(e => e.Id == employeeId && !e.IsDeleted) ??
                    throw new Exception($"Employee with ID {employeeId} not found.");

                bool isAlreadyAbsent = await context.DailyEmpAbsences
                    .AnyAsync(a => a.EmployeeId == employeeId && a.Date == DateOnly.FromDateTime(DateTime.Now.Date) && !a.IsDeleted);
    
                if (isAlreadyAbsent)
                    throw new Exception($"Employee {employee.Name} is already absent today.");

                var empAbsenceDto = new WDailyEmpAbsenceDto();
                empAbsenceDto.EmployeeId = employeeId;

                var empAbsence = empAbsenceDto.MapToAbsence();
                dailyEmpAbsences.Add(empAbsence);
            }
            await context.DailyEmpAbsences.AddRangeAsync(dailyEmpAbsences);
            await context.SaveChangesAsync();
            return dailyEmpAbsences.MapToRAbsenceDtos();
        }

        public async Task<RDailyEmpAbsenceDto> UpdateAbsenceAsync(int id, WDailyEmpAbsenceDto absenceDto)
        {
            var absence = await context.DailyEmpAbsences
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted) ??
                throw new Exception("Absence not found.");

            var employee = await context.DailyEmployees.FindAsync(absenceDto.EmployeeId) ??
                throw new Exception($"Employee with ID {absenceDto.EmployeeId} not found.");

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
