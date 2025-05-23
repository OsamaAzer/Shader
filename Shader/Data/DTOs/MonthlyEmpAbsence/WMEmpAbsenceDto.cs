using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.MonthlyEmpAbsence
{
    public class WMEmpAbsenceDto
    {
        [Required]
        public int EmployeeId { get; set; }
        public string? Reason { get; set; }
    }
}
