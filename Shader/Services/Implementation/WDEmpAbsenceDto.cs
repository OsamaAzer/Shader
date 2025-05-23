using System.ComponentModel.DataAnnotations;

namespace Shader.Services.Implementation
{
    public class WDEmpAbsenceDto
    {
        [Required]
        public int EmployeeId { get; set; }
        public string? Reason { get; set; }
    }
}