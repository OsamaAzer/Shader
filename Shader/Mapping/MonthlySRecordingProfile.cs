using Shader.Data.DTOs.MonthlySalaryRecording;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class MonthlySRecordingProfile
    {
        public static RMonthlySRecordingDto ToRMonthlySRecordingDto(this MonthlyEmpSalaryRecording monthlySalaryRecording)
        {
            return new RMonthlySRecordingDto
            {
                Id = monthlySalaryRecording.Id,
                EmployeeName = monthlySalaryRecording.Employee.Name,
                BaseSalary = monthlySalaryRecording.BaseSalary,
                BorrowedAmount = monthlySalaryRecording.BorrowedAmount,
                DeductionAmount = monthlySalaryRecording.DeductionAmount,
                Date = monthlySalaryRecording.Date
            };
        }

        public static IEnumerable<RMonthlySRecordingDto> ToRMonthlySRecordingDtos(this IEnumerable<MonthlyEmpSalaryRecording> monthlySalaryRecordings)
        {
            return monthlySalaryRecordings.Select(monthlySalaryRecording => monthlySalaryRecording.ToRMonthlySRecordingDto()).ToList();
        }

        public static MonthlyEmpSalaryRecording ToMonthlySRecording(this WMonthlySRecordingDto monthlySRecordingDto, MonthlyEmpSalaryRecording? monthlySRecording = null)
        {
            monthlySRecording ??= new MonthlyEmpSalaryRecording();
            monthlySRecording.EmployeeId = monthlySRecordingDto.EmployeeId;
            monthlySRecording.BaseSalary = monthlySRecording.Employee.BaseSalary;
            monthlySRecording.BorrowedAmount = monthlySRecording.Employee.BorrowedAmount;

            if (monthlySRecording.Date == default)
                monthlySRecording.Date = DateOnly.FromDateTime(DateTime.Now);
            
            return monthlySRecording;
        }
    }
}
