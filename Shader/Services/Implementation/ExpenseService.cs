using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs;
using Shader.Data.Entities;
using Shader.Mapping;
using Shader.Services.Abstraction;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shader.Services.Implementation
{
    public class ExpenseService(ShaderContext context) : IExpenseService
    {
        public async Task<IEnumerable<RExpenseDTO>> GetExpensesByDateAndTimeRangeAsync
            (DateOnly? startDate, DateOnly? endDate, TimeOnly? startTime, TimeOnly? endTime)
        {
            var todayDate = DateOnly.FromDateTime(DateTime.Now);
            IQueryable<Expense> query = context.Expenses.AsQueryable();

            if (startDate is not null && endDate is not null)
                query = query.Where(e => e.Date >= startDate && e.Date <= endDate);

            if (startTime is not null && endTime is not null)
                query = query.Where(e => e.Date == todayDate)
                             .Where(e => e.Time >= startTime && e.Time <= endTime);

            var expensesDTO = query.OrderByDescending(e => e.Date)
                                   .OrderByDescending(e => e.Time)
                                   .Map<Expense, RExpenseDTO>().ToList();
            return await Task.FromResult<IEnumerable<RExpenseDTO>>(expensesDTO);
        }
        public async Task<IEnumerable<RExpenseDTO>> GetExpensesByDateAsync(DateOnly date)
        {
            var expenseDto = context.Expenses
                .Where(e => e.Date == date)
                .OrderByDescending(e => e.Date)
                .OrderByDescending(e => e.Time)
                .Map<Expense, RExpenseDTO>().ToList();
            return await Task.FromResult<IEnumerable<RExpenseDTO>>(expenseDto);
        }
        public async Task<IEnumerable<RExpenseDTO>> GetAllExpensesAsync()
        {
            var expensesDTO = context.Expenses
                .OrderByDescending(e => e.Date)
                .OrderByDescending(e => e.Time)
                .Map<Expense, RExpenseDTO>().ToList();
            return await Task.FromResult<IEnumerable<RExpenseDTO>>(expensesDTO);
        }
        public async Task<RExpenseDTO> GetExpenseByIdAsync(int id)
        {
            var expense = await context.Expenses.FindAsync(id);
            if (expense is null) return null;
            var dto = expense.Map<Expense, RExpenseDTO>();
            if (dto is null) return await Task.FromResult<RExpenseDTO>(null);
            return dto;
        }
        public async Task<RExpenseDTO> AddExpenseAsync(WExpenseDTO dto)
        {
            var expense = dto.Map<WExpenseDTO, Expense>();
            expense.Date = DateOnly.FromDateTime(DateTime.Now);
            expense.Time = TimeOnly.FromDateTime(DateTime.Now);
            await context.Expenses.AddAsync(expense);
            await context.SaveChangesAsync();
            return expense.Map<Expense, RExpenseDTO>();
        }
        public async Task<RExpenseDTO> UpdateExpenseAsync(int id, WExpenseDTO dto)
        {
            var existingExpense = await context.Expenses.FindAsync(id);
            if (existingExpense is null) return null;
            dto.Map(existingExpense);
            context.Expenses.Update(existingExpense);
            await context.SaveChangesAsync();
            return existingExpense.Map<Expense, RExpenseDTO>();
        }
        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var existingExpense = await context.Expenses.FindAsync(id);
            if (existingExpense is null) return false;
            context.Expenses.Remove(existingExpense);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
