using Shader.Data.DTOs.DailyEmpAbsence;
using Shader.Data.DTOs.MonthlyEmpAbsence;
using Shader.Data.Entities;
using Shader.Services.Implementation;

namespace Shader.Mapping
{
    public static class DailyEmpAbsenceProfile
    {
        public static RDailyEmpAbsenceDto MapToRAbsenceDto(this DailyEmployeeAbsence absence)
        {
            return new RDailyEmpAbsenceDto
            {
                Id = absence.Id,
                EmployeeName = absence.Employee.Name,
                Date = absence.Date
            };
        }

        public static IEnumerable<RDailyEmpAbsenceDto> MapToRAbsenceDtos(this IEnumerable<DailyEmployeeAbsence> absences)
        {
            return absences.Select(MapToRAbsenceDto);
        }

        public static DailyEmployeeAbsence MapToAbsence(this WDailyEmpAbsenceDto absenceDto, DailyEmployeeAbsence? absence = null)
        {
            absence ??= new DailyEmployeeAbsence();
            absence.EmployeeId = absenceDto.EmployeeId;
            if (absence.Date == default)
            {
                absence.Date = DateOnly.FromDateTime(DateTime.Now);
            }
            return absence;
        }

        public static DailyEmployeeAbsence MapFromPastToDailyEmpAbsence(this WDailyPastAbsenceDto absenceDto)
        {
            var absence = new DailyEmployeeAbsence();
            absence.EmployeeId = absenceDto.EmployeeId;
            if (absenceDto.AbsenceDate >= DateOnly.FromDateTime(DateTime.Now))
                throw new Exception($"Absence date should be in the past!!");
            absence.Date = absenceDto.AbsenceDate;
            return absence;
        }
    }
}
