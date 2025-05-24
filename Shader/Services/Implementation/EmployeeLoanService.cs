using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.Loan;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class EmployeeLoanService(ShaderContext context) : IEmployeeLoanService
    {
        public async Task<PagedResponse<RLoanDto>> GetAllLoansAsync(int pageNumber, int pageSize)
        {
            var loans = await context.EmployeeLoans
                .Include(l => l.Employee)
                .Where(l => !l.IsDeleted)
                .ToListAsync();

            return loans.MapToRLoanDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RLoanDto>> GetLoansByDateRangeAsync
            (DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate == default || endDate == default)
                throw new Exception("Start date and end date are both required.");

            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var loans = await context.EmployeeLoans
                .Include(l => l.Employee)
                .Where(l => l.Date >= startDate && l.Date <= endDate && !l.IsDeleted)
                .ToListAsync();

            return loans.MapToRLoanDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async  Task<PagedResponse<RLoanDto>> GetLoansByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize)
        {
            var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == employeeId && !e.IsDeleted) ??
                throw new Exception($"The employee with ID {employeeId} does not exist!");

            var loans = await context.EmployeeLoans
                .Include(l => l.Employee)
                .Where(l => l.EmployeeId == employeeId && !l.IsDeleted)
                .ToListAsync();

            return loans.MapToRLoanDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<PagedResponse<RLoanDto>> GetLoansForEmployeeByDateRangeAsync
            (int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize)
        {
            if (startDate == default || endDate == default)
                throw new Exception("Start date and end date are both required.");

            if (startDate >= endDate)
                throw new Exception("Start date must be less than end date.");

            var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == employeeId && !e.IsDeleted) ??
                throw new Exception($"The employee with ID {employeeId} does not exist!");

            var loans = await context.EmployeeLoans
                .Include(l => l.Employee)
                .Where(l => l.EmployeeId == employeeId && l.Date >= startDate && l.Date<= endDate && !l.IsDeleted)
                .ToListAsync();

            return loans.MapToRLoanDtos().CreatePagedResponse(pageNumber, pageSize);
        }

        public async Task<RLoanDto> GetLoanByIdAsync(int id)
        {
            var loan = await context.EmployeeLoans
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted) ??
                throw new Exception($"Loan with ID {id} not found.");

            return loan.MapToRLoanDto();
        }

        public async Task<RLoanDto> AddLoanAsync(WLoanDto loanDto)
        {
            var employee = await context.MonthlyEmployees
               .FirstOrDefaultAsync(e => e.Id == loanDto.EmployeeId && !e.IsDeleted) ??
               throw new Exception($"The employee with ID {loanDto.EmployeeId} does not exist!");

            if (loanDto.Amount <= 0)
                throw new Exception("Loan amount must be greater than zero.");
            if (loanDto.Amount > employee.BaseSalary)
                throw new Exception($"Loan amount {loanDto.Amount} exceeds employee's base salary {employee.BaseSalary}.");
            if (loanDto.Amount > employee.RemainingSalary)
                throw new Exception($"Loan amount {loanDto.Amount} exceeds employee's Remaining salary {employee.RemainingSalary}.");

            var loan = loanDto.MapToLoan();
            loan.Employee = employee; 
            employee.BorrowedAmount += loan.Amount;
            employee.BorrowedAmount = Math.Max(employee.BorrowedAmount, 0); // Ensure borrowed amount is not negative
            context.MonthlyEmployees.Update(employee); 
            await context.EmployeeLoans.AddAsync(loan);
            await context.SaveChangesAsync();
            return loan.MapToRLoanDto();
        }

        public async Task<RLoanDto> UpdateLoanAsync(int id, WLoanDto loanDto)
        {
            var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == loanDto.EmployeeId && !e.IsDeleted) ??
                throw new Exception($"The employee with ID {loanDto.EmployeeId} does not exist!");

            var loan = await context.EmployeeLoans
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted) ??
                throw new Exception($"Loan with ID {id} not found.");

            if (loanDto.Amount <= 0)
                throw new Exception("Loan amount must be greater than zero.");
            if (loanDto.Amount > employee.BaseSalary)
                throw new Exception($"Loan amount {loanDto.Amount} exceeds employee's base salary {employee.BaseSalary}.");
            if (loanDto.Amount > employee.RemainingSalary)
                throw new Exception($"Loan amount {loanDto.Amount} exceeds employee's remaining salary {employee.RemainingSalary}.");

            if (loan.EmployeeId != loanDto.EmployeeId)
            {
                var removedEmployee = await context.MonthlyEmployees
                    .FirstOrDefaultAsync(e => e.Id == loan.EmployeeId && !e.IsDeleted);

                if (removedEmployee != null)
                {
                    removedEmployee.BorrowedAmount -= loan.Amount; 
                    context.MonthlyEmployees.Update(removedEmployee);
                }
                employee.BorrowedAmount += loanDto.Amount;
                context.MonthlyEmployees.Update(employee);
            }
            else 
            {                
                employee.BorrowedAmount -= loan.Amount;
                employee.BorrowedAmount += loanDto.Amount;
                context.MonthlyEmployees.Update(employee);
            }
            loanDto.MapToLoan(loan);
            context.EmployeeLoans.Update(loan);
            await context.SaveChangesAsync();
            return loan.MapToRLoanDto();
        }

        public async Task<bool> DeleteLoanAsync(int id)
        {
            var loan = await context.EmployeeLoans
                 .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted) ??
             throw new Exception($"Loan with ID {id} not found.");

            var employee = await context.MonthlyEmployees
                .FirstOrDefaultAsync(e => e.Id == loan.EmployeeId && !e.IsDeleted);

            if (employee != null)
            {
                employee.BorrowedAmount -= loan.Amount; 
                context.MonthlyEmployees.Update(employee);
            }

            loan.IsDeleted = true;
            context.EmployeeLoans.Update(loan);
            return await context.SaveChangesAsync() > 0;
        }

    }
}
