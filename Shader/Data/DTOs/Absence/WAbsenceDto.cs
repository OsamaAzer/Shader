using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.Absence
{
    public class WAbsenceDto
    {
        [Required]
        public int EmployeeId { get; set; }
        public string? Reason { get; set; }
    }
}
