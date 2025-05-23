using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.DailySalaryRecording
{
    public class WDailySRecordingDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
