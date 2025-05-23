using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.DailyEmpAbsence
{
    public class WDEmpAbsence
    {
        [Required]
        public int EmployeeId { get; set; }
        public string? Reason { get; set; }
    }
}
