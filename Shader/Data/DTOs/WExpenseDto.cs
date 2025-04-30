using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs
{
    public class WExpenseDTO
    {
        [Required]
        public ExpenseType Type { get; set; } // نوع المصروف
        [Required]
        public decimal Amount { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
