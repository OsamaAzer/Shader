using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;
using Shader.Services.Implementation;

namespace Shader.Mapping
{
    public static class DailyEmpAbsenceProfile
    {
        public static RDEmpAbsenceDto MapToRAbsenceDto(this DailyEmployeeAbsence absence)
        {
            return new RDEmpAbsenceDto
            {
                Id = absence.Id,
                EmployeeName = absence.Employee.Name,
                Reason = absence.Reason,
                Date = absence.Date
            };
        }

        public static IEnumerable<RDEmpAbsenceDto> MapToRAbsenceDtos(this IEnumerable<DailyEmployeeAbsence> absences)
        {
            return absences.Select(MapToRAbsenceDto);
        }

        public static DailyEmployeeAbsence MapToAbsence(this WDEmpAbsenceDto absenceDto, DailyEmployeeAbsence? absence = null)
        {
            absence ??= new DailyEmployeeAbsence();
            absence.Reason = absenceDto.Reason;
            absence.EmployeeId = absenceDto.EmployeeId;
            return absence;
        }
    }
}
