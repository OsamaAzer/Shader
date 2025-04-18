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
        public async Task<IEnumerable<RExpenseDTO>> GetExpensesByDateAndTimeRangeAsync(DateOnly? startDate, DateOnly? endDate, TimeOnly? startTime, TimeOnly? endTime)
        {
            var todayDate = DateOnly.FromDateTime(DateTime.Now);
            IQueryable<Expense> query = context.Expenses.AsQueryable();

            if (startDate is null && endDate is null && startTime is null && endTime is null)
                query = query.Where(e => e.Date == todayDate);

            else if (startTime is null && endTime is null)
                query = query.Where(e => e.Date >= startDate && e.Date <= endDate);

            else if (startDate is null && endDate is null)
                query = query.Where(e => e.Date == todayDate)
                             .Where(e => e.Time >= startTime && e.Time <= endTime);

            var expensesDTO = query.OrderByDescending(e => e.Date)
                                   .OrderByDescending(e => e.Time)
                                   .ToDTO<Expense, RExpenseDTO>().ToList();
            return await Task.FromResult<IEnumerable<RExpenseDTO>>(expensesDTO);
        }
        public async Task<IEnumerable<RExpenseDTO>> GetExpensesByDateAsync(DateOnly date)
        {
            var expenseDto = context.Expenses
                .Where(e => e.Date == date)
                .OrderByDescending(e => e.Time)
                .OrderByDescending(e => e.Time)
                .ToDTO<Expense, RExpenseDTO>().ToList();
            return await Task.FromResult<IEnumerable<RExpenseDTO>>(expenseDto);
        }
        public async Task<IEnumerable<RExpenseDTO>> GetAllExpensesAsync()
        {
            var expensesDTO = context.Expenses
                .OrderByDescending(e => e.Date)
                .OrderByDescending(e => e.Time)
                .ToDTO<Expense, RExpenseDTO>().ToList();
            return await Task.FromResult<IEnumerable<RExpenseDTO>>(expensesDTO);
        }
        public async Task<RExpenseDTO> GetExpenseByIdAsync(int id)
        {
            var expenseDto = await context.Expenses
                .Where(e => e.Id == id)
                .Select(e => e.ToDTO<Expense, RExpenseDTO>())
                .FirstOrDefaultAsync();
            if (expenseDto is null) return await Task.FromResult<RExpenseDTO>(null);
            return expenseDto;
        }
        public async Task<bool> AddExpenseAsync(WExpenseDTO dto)
        {
            if (dto.date is null)
                dto.date = DateOnly.FromDateTime(DateTime.Now);

            if (dto.time is null)
                dto.time = TimeOnly.FromDateTime(DateTime.Now);

            var expense = dto.ToEntity<Expense, WExpenseDTO>();
            await context.Expenses.AddAsync(expense);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateExpenseAsync(int id, WExpenseDTO dto)
        {
            var existingExpense = await context.Expenses.FindAsync(id);
            if (existingExpense is null) return false;
            dto.ToEntity(existingExpense);
            context.Expenses.Update(existingExpense);
            return await context.SaveChangesAsync() > 0;
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
