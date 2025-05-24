using Shader.Data.DTOs.Loan;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class EmployeeLoanProfile
    {
        public static RLoanDto MapToRLoanDto(this EmployeeLoan loan)
        {
            return new RLoanDto
            {
                Id = loan.Id,
                Amount = loan.Amount,
                Date = loan.Date,
                EmployeeName = loan.Employee.Name
            };
        }

        public static IEnumerable<RLoanDto> MapToRLoanDtos(this IEnumerable<EmployeeLoan> loans)
        {
            return loans.Select(loan => loan.MapToRLoanDto()).ToList();
        }

        public static EmployeeLoan MapToLoan(this WLoanDto loanDto, EmployeeLoan? loan = null)
        {
            loan ??= new EmployeeLoan();
            loan.EmployeeId = loanDto.EmployeeId;
            if (loanDto.Amount <= 0)
            {
                throw new ArgumentException("Loan amount must be greater than zero.", nameof(loanDto.Amount));
            }
            else
            {
                loan.Amount = loanDto.Amount;
            }
            if (loan.Date == default)
            {
                loan.Date = DateOnly.FromDateTime(DateTime.Now);
            }
            return loan;
        }
    }
}
