using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.Loan;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class LoanService(ShaderContext context) : ILoanService
    {
        public async Task<IEnumerable<RLoanDto>> GetAllLoansAsync()
        {
            var loans = await context.Loans
                .Where(l => !l.IsDeleted)
                .ToListAsync();

            return loans.MapToRLoanDtos();
        }

        public async Task<IEnumerable<RLoanDto>> GetLoansByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            var loans = await context.Loans
                .Where(l => DateOnly.FromDateTime(l.Date) >= startDate && DateOnly.FromDateTime(l.Date) <= endDate && !l.IsDeleted)
                .ToListAsync();

            return loans.MapToRLoanDtos();
        }

        public async  Task<IEnumerable<RLoanDto>> GetLoansByEmployeeIdAsync(int employeeId)
        {
            var loans = await context.Loans
                .Where(l => l.EmployeeId == employeeId && !l.IsDeleted)
                .ToListAsync();

            return loans.MapToRLoanDtos();
        }

        public async Task<IEnumerable<RLoanDto>> GetLoansForEmployeeByDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate)
        {
            var loans = await context.Loans
                .Where(l => l.EmployeeId == employeeId && DateOnly.FromDateTime(l.Date) >= startDate && DateOnly.FromDateTime(l.Date) <= endDate && !l.IsDeleted)
                .ToListAsync();

            return loans.MapToRLoanDtos();
        }
        public async Task<RLoanDto> GetLoanByIdAsync(int id)
        {
            var loan = await context.Loans
                .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted) ??
                throw new Exception($"Loan with ID {id} not found.");

            return loan.MapToRLoanDto();
        }
        public async Task<RLoanDto> AddLoanAsync(WLoanDto loanDto)
        {
            var loan = loanDto.MapToLoan();
            loan.Date = DateTime.Now;
            await context.Loans.AddAsync(loan);
            await context.SaveChangesAsync();
            return loan.MapToRLoanDto();
        }
        public async Task<RLoanDto> UpdateLoanAsync(int id, WLoanDto loanDto)
        {
            var loan = await context.Loans
                .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted) ??
                throw new Exception($"Loan with ID {id} not found.");

            loanDto.MapToLoan(loan);
            context.Loans.Update(loan);
            await context.SaveChangesAsync();
            return loan.MapToRLoanDto();
        }
        public async Task<bool> DeleteLoanAsync(int id)
        {
            var loan = await context.Loans
                 .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted) ??
             throw new Exception($"Loan with ID {id} not found.");

            loan.IsDeleted = true;
            context.Loans.Update(loan);
            return await context.SaveChangesAsync() > 0;
        }

    }
}
