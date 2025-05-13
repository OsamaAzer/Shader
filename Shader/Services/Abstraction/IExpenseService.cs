using Shader.Data.Dtos.Expense;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface IExpenseService
    {
        Task<PagedResponse<RExpenseDto>> GetExpensesByDateAndTimeRangeAsync(DateOnly startDate, DateOnly endDate, int pageNumber, int pageSize);
        Task<PagedResponse<RExpenseDto>> GetExpensesByDateAsync(DateOnly date, int pageNumber, int pageSize);
        Task<PagedResponse<RExpenseDto>> GetAllExpensesAsync(int pageNumber, int pageSize);
        Task<RExpenseDto> GetExpenseByIdAsync(int id);
        Task<RExpenseDto> AddExpenseAsync(WExpenseDto dto);
        Task<RExpenseDto> UpdateExpenseAsync(int id, WExpenseDto dto);
        Task<bool> DeleteExpenseAsync(int id);
    }
}
