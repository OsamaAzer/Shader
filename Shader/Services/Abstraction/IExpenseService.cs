using Shader.Data.Dtos.Expense;

namespace Shader.Services.Abstraction
{
    public interface IExpenseService
    {
        Task<IEnumerable<RExpenseDto>> GetExpensesByDateAndTimeRangeAsync(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<RExpenseDto>> GetExpensesByDateAsync(DateOnly date);
        Task<IEnumerable<RExpenseDto>> GetAllExpensesAsync();
        Task<RExpenseDto> GetExpenseByIdAsync(int id);
        Task<RExpenseDto> AddExpenseAsync(WExpenseDto dto);
        Task<RExpenseDto> UpdateExpenseAsync(int id, WExpenseDto dto);
        Task<bool> DeleteExpenseAsync(int id);
    }
}
