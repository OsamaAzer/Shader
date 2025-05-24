using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.Expense;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shader.Services.Implementation
{
    public class ExpenseService(ShaderContext context) : IExpenseService
    {
        List<ExpenseType> expenses = 
            [ExpenseType.Rent, ExpenseType.Maintenance, ExpenseType.Transportation, ExpenseType.MonthlySalaries,
            ExpenseType.DailySalaries, ExpenseType.Drinks, ExpenseType.Electricity, ExpenseType.Food, ExpenseType.Other]; 
        public async Task<PagedResponse<RExpenseDto>> GetExpensesByDateAndTimeRangeAsync
        (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate == default || endDate == default)
                throw new Exception("Start date and end date are both required.");

            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");
            
            var expenses = await context.Expenses
                .Where(e => !e.IsDeleted && DateOnly.FromDateTime(e.Date) >= startDate && DateOnly.FromDateTime(e.Date) <= endDate)
                .OrderByDescending(e => e.Date).ToListAsync();

            return expenses.MapToRExpenseDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RExpenseDto>> GetExpensesByDateAsync(DateOnly date, int pageNumber, int pageSize)
        {
            var expenses = await context.Expenses
                .Where(e => !e.IsDeleted && DateOnly.FromDateTime(e.Date) == date)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return expenses.MapToRExpenseDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RExpenseDto>> GetAllExpensesAsync(int pageNumber, int pageSize)
        {
            var expenses = await context.Expenses
                .Where (e => !e.IsDeleted)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return expenses.MapToRExpenseDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<RExpenseDto> GetExpenseByIdAsync(int id)
        {
            var expense = await context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted) ??
                throw new Exception("This expense does not exist!");

            return expense.MapToRExpenseDto();
        }

        public async Task<RExpenseDto> AddExpenseAsync(WExpenseDto dto)
        {
            if (dto.Amount <= 0) 
                throw new Exception("Amount must be greater than zero.");
            var expense = dto.MapToExpense();
            if (expenses.Any(e => e != dto.Type))
            {
                expense.Type = ExpenseType.Other;
            }
            expense.Date = DateTime.Now;
            await context.Expenses.AddAsync(expense);
            await context.SaveChangesAsync();
            return expense.MapToRExpenseDto();
        }

        public async Task<RExpenseDto> UpdateExpenseAsync(int id, WExpenseDto dto)
        {
            if (dto.Amount <= 0) 
                throw new Exception("Amount must be greater than zero.");

            var expense = await context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted) ??
                throw new Exception("This expense does not exist!");

            dto.MapToExpense(expense);
            if (expenses.Any(e => e != dto.Type))
            {
                expense.Type = ExpenseType.Other;
            }
            context.Expenses.Update(expense);
            await context.SaveChangesAsync();
            return expense.MapToRExpenseDto();
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var existingExpense = await context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted) ??
                throw new Exception("This expense does not exist!");

            existingExpense.IsDeleted = true;
            context.Expenses.Update(existingExpense);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
