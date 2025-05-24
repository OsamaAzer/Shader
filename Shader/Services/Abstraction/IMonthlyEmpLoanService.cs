using Shader.Data.DTOs.Loan;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IEmployeeLoanService
    {
        Task<PagedResponse<RLoanDto>> GetAllLoansAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RLoanDto>> GetLoansByDateRangeAsync(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<PagedResponse<RLoanDto>> GetLoansByEmployeeIdAsync(int employeeId, int pageNumber, int pageSize);
        Task<PagedResponse<RLoanDto>> GetLoansForEmployeeByDateRangeAsync(int employeeId, DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<RLoanDto> AddLoanAsync(WLoanDto loan);
        Task<RLoanDto> UpdateLoanAsync(int id, WLoanDto loan);
        Task<bool> DeleteLoanAsync(int id);
    }
}
