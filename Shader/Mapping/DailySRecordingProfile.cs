using Shader.Data.DTOs.DailySalaryRecording;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class DailySRecordingProfile
    {
        public static RDailySRecordingDto ToRDailySRecordingDto(this DailySalaryRecording dailySalaryRecording)
        {
            return new RDailySRecordingDto
            {
                Id = dailySalaryRecording.Id,
                EmployeeName = dailySalaryRecording.Employee.Name,
                Date = dailySalaryRecording.Date
            };
        }

        public static IEnumerable<RDailySRecordingDto> ToRDailySRecordingDtos(this IEnumerable<DailySalaryRecording> dailySalaryRecordings)
        {
            return dailySalaryRecordings.Select(dailySalaryRecording => dailySalaryRecording.ToRDailySRecordingDto()).ToList();
        }

        public static DailySalaryRecording ToDailySRecording(this WDailySRecordingDto dailySRecordingDto, DailySalaryRecording? dailySRecording = null)
        {
            dailySRecording ??= new DailySalaryRecording();
            dailySRecording.EmployeeId = dailySRecordingDto.EmployeeId;
            return dailySRecording;
        }
    }
}
