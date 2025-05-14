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
            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");
            
            var expensesDto = context.Expenses
                .Where(e => !e.IsDeleted && DateOnly.FromDateTime(e.Date) >= startDate && DateOnly.FromDateTime(e.Date) <= endDate)
                .OrderByDescending(e => e.Date)
                .Map<Expense, RExpenseDto>().ToList();
            return  await Task.FromResult(expensesDto.CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RExpenseDto>> GetExpensesByDateAsync(DateOnly date, int pageNumber, int pageSize)
        {
            var expensesDto = context.Expenses
                .Where(e => !e.IsDeleted && DateOnly.FromDateTime(e.Date) == date)
                .OrderByDescending(e => e.Date)
                .Map<Expense, RExpenseDto>().ToList();
            return await Task.FromResult(expensesDto.CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<PagedResponse<RExpenseDto>> GetAllExpensesAsync(int pageNumber, int pageSize)
        {
            var expensesDto = context.Expenses
                .Where (e => !e.IsDeleted)
                .OrderByDescending(e => e.Date)
                .Map<Expense, RExpenseDto>().ToList();
            return  await Task.FromResult(expensesDto.CreatePagedResponse(pageNumber, pageSize));
        }
        public async Task<RExpenseDto> GetExpenseByIdAsync(int id)
        {
            var expense = await context.Expenses
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync()??
                throw new Exception("This expense does not exist!");
            return expense.Map<Expense, RExpenseDto>();
        }
        public async Task<RExpenseDto> AddExpenseAsync(WExpenseDto dto)
        {
            if (dto.Amount <= 0) 
                throw new Exception("Amount must be greater than zero.");
            var expense = dto.Map<WExpenseDto, Expense>();
            if (expenses.Any(e => e != dto.Type))
            {
                expense.Type = ExpenseType.Other;
            }
            expense.Date = DateTime.Now;
            await context.Expenses.AddAsync(expense);
            await context.SaveChangesAsync();
            return expense.Map<Expense, RExpenseDto>();
        }
        public async Task<RExpenseDto> UpdateExpenseAsync(int id, WExpenseDto dto)
        {
            if (dto.Amount <= 0) 
                throw new Exception("Amount must be greater than zero.");

            var existingExpense = await context.Expenses
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync()??
                throw new Exception("This expense does not exist!");

            dto.Map(existingExpense);
            if (expenses.Any(e => e != dto.Type))
            {
                existingExpense.Type = ExpenseType.Other;
            }
            context.Expenses.Update(existingExpense);
            await context.SaveChangesAsync();
            return existingExpense.Map<Expense, RExpenseDto>();
        }
        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var existingExpense = await context.Expenses
                .Where(e => e.Id == id && !e.IsDeleted)
                .FirstOrDefaultAsync() ??
                throw new Exception("This expense does not exist!");
            existingExpense.IsDeleted = true;
            context.Expenses.Update(existingExpense);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
