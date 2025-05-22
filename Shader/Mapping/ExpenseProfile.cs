using Shader.Data.Dtos.Expense;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class ExpenseProfile
    {
        public static Expense MapToExpense(this WExpenseDto expenseDto, Expense? expense = null)
        {
            expense ??= new Expense();
            expense.Amount = expenseDto.Amount;
            expense.Description = expenseDto.Description;
            expense.Type = expenseDto.Type;
            expense.IsDeleted = false;
            return expense;
        }

        public static RExpenseDto MapToRExpenseDto(this Expense expense)
        {
            return new RExpenseDto
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                Type = expense.Type
            };
        }

        public static IEnumerable<RExpenseDto> MapToRExpenseDtos(this IEnumerable<Expense> expenses)
        {
            return expenses.Select(e => new RExpenseDto
            {
                Id = e.Id,
                Amount = e.Amount,
                Date = e.Date,
                Description = e.Description,
                Type = e.Type
            });
        }
    }
}
