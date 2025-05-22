using Shader.Data.DTOs.Absence;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class AbsenceProfile
    {
        public static IEnumerable<RAbsenceDto> MapToRAbsenceDtos(this IEnumerable<Absence> absences)
        {
            return absences.Select(a => new RAbsenceDto
            {
                Id = a.Id,
                EmployeeName = a.Employee.Name,
                Date = a.Date,
                Reason = a.Reason
            });
        }
        public static RAbsenceDto MapToRAbsenceDto(this Absence absence)
        {
            return new RAbsenceDto
            {
                Id = absence.Id,
                EmployeeName = absence.Employee.Name,
                Date = absence.Date,
                Reason = absence.Reason
            };
        }
        public static Absence MapToAbsence(this WAbsenceDto absenceDto, Absence? absence = null)
        {
            absence ??= new Absence();
            absence.EmployeeId = absenceDto.EmployeeId;
            absence.Reason = absenceDto.Reason;
            return absence;
        }
    }
}
