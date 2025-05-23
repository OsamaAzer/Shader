using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.MonthlySalaryRecording
{
    public class WMonthlySRecordingDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
