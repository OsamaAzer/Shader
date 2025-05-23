using Shader.Data.DTOs.Loan;

namespace Shader.Services.Abstraction
{
    public interface ILoanService
    {
        Task<IEnumerable<RLoanDto>> GetAllLoansAsync();
        Task<IEnumerable<RLoanDto>> GetLoansByDateRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<RLoanDto>> GetLoansByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<RLoanDto>> GetLoansForEmployeeByDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate);
        Task<RLoanDto> AddLoanAsync(WLoanDto loan);
        Task<RLoanDto> UpdateLoanAsync(int id, WLoanDto loan);
        Task<bool> DeleteLoanAsync(int id);
    }
}
