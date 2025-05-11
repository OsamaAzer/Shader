using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.Expense;
using Shader.Data.Entities;
using Shader.Mapping;
using Shader.Services.Abstraction;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shader.Services.Implementation
{
    public class ExpenseService(ShaderContext context) : IExpenseService
    {
        public async Task<IEnumerable<RExpenseDto>> GetExpensesByDateAndTimeRangeAsync
            (DateOnly startDate, DateOnly endDate)
        {
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");
            
            var expensesDto =  context.Expenses
                .Where(e => !e.IsDeleted && DateOnly.FromDateTime(e.Date) >= startDate && DateOnly.FromDateTime(e.Date) <= endDate)
                .OrderByDescending(e => e.Date)
                .OrderByDescending(e => e.Date.Hour)
                .Map<Expense, RExpenseDto>().ToList();

            return await Task.FromResult<IEnumerable<RExpenseDto>>(expensesDto);
        }
        public async Task<IEnumerable<RExpenseDto>> GetExpensesByDateAsync(DateOnly date)
        {
            var expenseDto = context.Expenses
                .Where(e => !e.IsDeleted && DateOnly.FromDateTime(e.Date) == date)
                .OrderByDescending(e => e.Date)
                .OrderByDescending(e => e.Date.Hour)
                .Map<Expense, RExpenseDto>().ToList();
            return await Task.FromResult<IEnumerable<RExpenseDto>>(expenseDto);
        }
        public async Task<IEnumerable<RExpenseDto>> GetAllExpensesAsync()
        {
            var expensesDto = context.Expenses
                .Where (e => !e.IsDeleted)
                .OrderByDescending(e => e.Date)
                .OrderByDescending(e => e.Date.Hour)
                .Map<Expense, RExpenseDto>().ToList();
            return await Task.FromResult<IEnumerable<RExpenseDto>>(expensesDto);
        }
        public async Task<RExpenseDto> GetExpenseByIdAsync(int id)
        {
            var expense = await context.Expenses
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync();
            if (expense is null) throw new Exception("This expense does not exist!");
            return expense.Map<Expense, RExpenseDto>();
        }
        public async Task<RExpenseDto> AddExpenseAsync(WExpenseDto dto)
        {
            var expense = dto.Map<WExpenseDto, Expense>();
            expense.Date = DateTime.Now;
            await context.Expenses.AddAsync(expense);
            await context.SaveChangesAsync();
            return expense.Map<Expense, RExpenseDto>();
        }
        public async Task<RExpenseDto> UpdateExpenseAsync(int id, WExpenseDto dto)
        {
            if (dto.Amount <= 0) throw new Exception("Amount must be greater than zero.");

            var existingExpense = await context.Expenses
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync();
            if (existingExpense is null) throw new Exception("This expense does not exist!");

            dto.Map(existingExpense);
            context.Expenses.Update(existingExpense);
            await context.SaveChangesAsync();
            return existingExpense.Map<Expense, RExpenseDto>();
        }
        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var existingExpense = await context.Expenses
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync();
            if (existingExpense is null) throw new Exception("This expense does not exist!");
            context.Expenses.Update(existingExpense);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
