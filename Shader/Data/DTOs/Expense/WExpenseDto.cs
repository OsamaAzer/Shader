using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.Expense
{
    public class WExpenseDto
    {
        [Required]
        public ExpenseType Type { get; set; } // نوع المصروف
        [Required]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
