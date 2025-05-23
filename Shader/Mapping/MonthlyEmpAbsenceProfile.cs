using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class MonthlyEmpAbsenceProfile
    {
        public static IEnumerable<RDEmpAbsenceDto> MapToRAbsenceDtos(this IEnumerable<MonthlyEmployeeAbsence> absences)
        {
            return absences.Select(a => new RDEmpAbsenceDto
            {
                Id = a.Id,
                EmployeeName = a.Employee.Name,
                Date = a.Date,
                Reason = a.Reason
            });
        }
        public static RDEmpAbsenceDto MapToRAbsenceDto(this MonthlyEmployeeAbsence absence)
        {
            return new RDEmpAbsenceDto
            {
                Id = absence.Id,
                EmployeeName = absence.Employee.Name,
                Date = absence.Date,
                Reason = absence.Reason
            };
        }
        public static MonthlyEmployeeAbsence MapToAbsence(this WMEmpAbsenceDto absenceDto, MonthlyEmployeeAbsence? absence = null)
        {
            absence ??= new MonthlyEmployeeAbsence();
            absence.EmployeeId = absenceDto.EmployeeId;
            absence.Reason = absenceDto.Reason;
            return absence;
        }
    }
}
