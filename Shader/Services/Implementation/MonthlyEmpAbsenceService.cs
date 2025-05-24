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
            var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == employeeId && !e.IsDeleted) ??
                throw new Exception($"The employee with ID {employeeId} does not exist!");

            var absences = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && !a.IsDeleted)
                .ToListAsync();

            return absences.MapToRAbsenceDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RMonthlyEmpAbsenceDto>> GetAbsencesByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate == default  || endDate == default)
                throw new Exception("Start date and end date are both required.");

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
            if (startDate == default || endDate == default)
                throw new Exception("Start date and end date are both required.");

            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == employeeId && !e.IsDeleted) ??
                throw new Exception($"The employee with ID {employeeId} does not exist!");

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

            var absences = new List<MonthlyEmpAbsence>();
            foreach (var employeeId in employeeIds)
            {
                var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == employeeId && !e.IsDeleted) ??
                throw new Exception($"The employee with ID {employeeId} does not exist!");

                var existingAbsence = await context.MonthlyEmpAbsences
                    .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == DateOnly.FromDateTime(DateTime.Now.Date) && !a.IsDeleted);

                if (existingAbsence != null)
                    throw new Exception($"Employee {employee.Name} is already absent today.");

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
            var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == absenceDto.EmployeeId && !e.IsDeleted) ??
                throw new Exception($"The employee with ID {absenceDto.EmployeeId} does not exist!");

            var absence = await context.MonthlyEmpAbsences
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted)??
                throw new Exception("Absence not found.");

            var existingAbsence = await context.MonthlyEmpAbsences
                   .FirstOrDefaultAsync(a => a.EmployeeId == absenceDto.EmployeeId && a.Date == DateOnly.FromDateTime(DateTime.Now.Date) && !a.IsDeleted);

            if (existingAbsence != null)
                throw new Exception($"Employee {employee.Name} is already absent this day.");

            absence = absenceDto.MapToAbsence(absence);
            context.MonthlyEmpAbsences.Update(absence);
            await context.SaveChangesAsync();
            return absence.MapToRAbsenceDto();
        }

        public async Task<bool> DeleteAbsenceAsync(int id)
        {
            var absence = await context.MonthlyEmpAbsences
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted) ??
                throw new Exception($"Absence with ID {id} not found.");

            absence.IsDeleted = true;
            context.MonthlyEmpAbsences.Update(absence);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
