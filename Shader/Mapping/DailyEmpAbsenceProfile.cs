using Shader.Data.DTOs.DailyEmpAbsence;
using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;
using Shader.Services.Implementation;

namespace Shader.Mapping
{
    public static class DailyEmpAbsenceProfile
    {
        public static RDailyEmpAbsenceDto MapToRAbsenceDto(this DailyEmpAbsence absence)
        {
            return new RDailyEmpAbsenceDto
            {
                Id = absence.Id,
                EmployeeName = absence.Employee.Name,
                Date = absence.Date
            };
        }

        public static IEnumerable<RDailyEmpAbsenceDto> MapToRAbsenceDtos(this IEnumerable<DailyEmpAbsence> absences)
        {
            return absences.Select(MapToRAbsenceDto);
        }

        public static DailyEmpAbsence MapToAbsence(this WDailyEmpAbsenceDto absenceDto, DailyEmpAbsence? absence = null)
        {
            absence ??= new DailyEmpAbsence();
            absence.EmployeeId = absenceDto.EmployeeId;
            if (absence.Date == default)
            {
                absence.Date = DateOnly.FromDateTime(DateTime.Now);
            }
            return absence;
        }

        public static DailyEmpAbsence MapFromPastToDailyEmpAbsence(this WDailyPastAbsenceDto absenceDto)
        {
            var absence = new DailyEmpAbsence();
            absence.EmployeeId = absenceDto.EmployeeId;
            if (absenceDto.AbsenceDate >= DateOnly.FromDateTime(DateTime.Now))
                throw new Exception($"Absence date should be in the past!!");
            absence.Date = absenceDto.AbsenceDate;
            return absence;
        }
    }
}
