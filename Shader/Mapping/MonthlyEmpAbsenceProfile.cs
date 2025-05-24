using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class MonthlyEmpAbsenceProfile
    {
        public static IEnumerable<RMonthlyEmpAbsenceDto> MapToRAbsenceDtos(this IEnumerable<MonthlyEmployeeAbsence> absences)
        {
            return absences.Select(a => new RMonthlyEmpAbsenceDto
            {
                Id = a.Id,
                EmployeeName = a.Employee.Name,
                Date = a.Date,
            });
        }
        public static RMonthlyEmpAbsenceDto MapToRAbsenceDto(this MonthlyEmployeeAbsence absence)
        {
            return new RMonthlyEmpAbsenceDto
            {
                Id = absence.Id,
                EmployeeName = absence.Employee.Name,
                Date = absence.Date,
            };
        }
        public static MonthlyEmployeeAbsence MapToAbsence(this WMonthlyEmpAbsenceDto absenceDto, MonthlyEmployeeAbsence? absence = null)
        {
            absence ??= new MonthlyEmployeeAbsence();
            absence.EmployeeId = absenceDto.EmployeeId;
            if(absence.Date == default)
            {
                absence.Date = DateOnly.FromDateTime(DateTime.Now);
            }
            return absence;
        }
    }
}
