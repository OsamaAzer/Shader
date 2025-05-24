using System.ComponentModel.DataAnnotations;

namespace Shader.Services.Implementation
{
    public class WDailyEmpAbsenceDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}