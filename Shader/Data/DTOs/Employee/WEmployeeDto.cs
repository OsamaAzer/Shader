using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.Employee
{
    public class WEmployeeDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; }
        public SalaryType SalaryType { get; set; } // نوع المرتب
        public decimal Salary { get; set; }
    }
}
