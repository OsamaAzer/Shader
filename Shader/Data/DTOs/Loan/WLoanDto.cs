using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.Loan
{
    public class WLoanDto
    {
        [Required]
        public int EmployeeId { get; set; }
        public decimal Amount { get; set; }
    }
}
