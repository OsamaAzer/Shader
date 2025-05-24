using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class MonthlyEmpAbsenceProfile
    {
        public static IEnumerable<RMonthlyEmpAbsenceDto> MapToRAbsenceDtos(this IEnumerable<MonthlyEmpAbsence> absences)
        {
            return absences.Select(a => new RMonthlyEmpAbsenceDto
            {
                Id = a.Id,
                EmployeeName = a.Employee.Name,
                Date = a.Date,
            });
        }
        public static RMonthlyEmpAbsenceDto MapToRAbsenceDto(this MonthlyEmpAbsence absence)
        {
            return new RMonthlyEmpAbsenceDto
            {
                Id = absence.Id,
                EmployeeName = absence.Employee.Name,
                Date = absence.Date,
            };
        }
        public static MonthlyEmpAbsence MapToAbsence(this WMonthlyEmpAbsenceDto absenceDto, MonthlyEmpAbsence? absence = null)
        {
            absence ??= new MonthlyEmpAbsence();
            absence.EmployeeId = absenceDto.EmployeeId;
            if(absence.Date == default)
            {
                absence.Date = DateOnly.FromDateTime(DateTime.Now);
            }
            return absence;
        }
    }
}
