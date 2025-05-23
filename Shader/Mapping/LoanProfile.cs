using Shader.Data.DTOs.Loan;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class LoanProfile
    {
        public static RLoanDto MapToRLoanDto(this Loan loan)
        {
            return new RLoanDto
            {
                Id = loan.Id,
                Amount = loan.Amount,
                Date = loan.Date,
                EmployeeName = loan.Employee.Name
            };
        }

        public static IEnumerable<RLoanDto> MapToRLoanDtos(this IEnumerable<Loan> loans)
        {
            return loans.Select(loan => loan.MapToRLoanDto()).ToList();
        }

        public static Loan MapToLoan(this WLoanDto loanDto, Loan? loan = null)
        {
            loan ??= new Loan();
            loan.Amount = loanDto.Amount;
            loan.EmployeeId = loanDto.EmployeeId;
            return loan;
        }
    }
}
