using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.MonthlyEmpAbsence
{
    public class WMonthlyEmpAbsenceDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
