using Shader.Data.DTOs.DailySalaryRecording;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class DailySRecordingProfile
    {
        public static RDailySRecordingDto ToRDailySRecordingDto(this DailyEmpSalaryRecording dailySalaryRecording)
        {
            return new RDailySRecordingDto
            {
                Id = dailySalaryRecording.Id,
                EmployeeName = dailySalaryRecording.Employee.Name,
                DailySalary = dailySalaryRecording.Employee.DailySalary,
                Date = dailySalaryRecording.Date
            };
        }

        public static IEnumerable<RDailySRecordingDto> ToRDailySRecordingDtos(this IEnumerable<DailyEmpSalaryRecording> dailySalaryRecordings)
        {
            return dailySalaryRecordings.Select(dailySalaryRecording => dailySalaryRecording.ToRDailySRecordingDto()).ToList();
        }

        public static DailyEmpSalaryRecording ToDailySRecording(this WDailySRecordingDto dailySRecordingDto, DailyEmpSalaryRecording? dailySRecording = null)
        {
            dailySRecording ??= new DailyEmpSalaryRecording();
            dailySRecording.EmployeeId = dailySRecordingDto.EmployeeId;
            if (dailySRecording.Date == default)
            {
                dailySRecording.Date = DateOnly.FromDateTime(DateTime.Now);
            }
            return dailySRecording;
        }
    }
}
