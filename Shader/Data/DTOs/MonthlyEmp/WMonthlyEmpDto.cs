using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.MonthlyEmp
{
    public class WMonthlyEmpDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; }
        public decimal BaseSalary { get; set; }
    }
}
