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
                Reason = a.Reason
            });
        }
        public static RMonthlyEmpAbsenceDto MapToRAbsenceDto(this MonthlyEmployeeAbsence absence)
        {
            return new RMonthlyEmpAbsenceDto
            {
                Id = absence.Id,
                EmployeeName = absence.Employee.Name,
                Date = absence.Date,
                Reason = absence.Reason
            };
        }
        public static MonthlyEmployeeAbsence MapToAbsence(this WMonthlyEmpAbsenceDto absenceDto, MonthlyEmployeeAbsence? absence = null)
        {
            absence ??= new MonthlyEmployeeAbsence();
            absence.EmployeeId = absenceDto.EmployeeId;
            absence.Reason = absenceDto.Reason;
            if(absence.Date == default)
            {
                absence.Date = DateOnly.FromDateTime(DateTime.Now);
            }
            return absence;
        }
    }
}
