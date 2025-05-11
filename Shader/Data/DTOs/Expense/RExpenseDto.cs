using Shader.Enums;

namespace Shader.Data.Dtos.Expense
{
    public class RExpenseDto
    {
        public ExpenseType Type { get; set; } // نوع المصروف
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
