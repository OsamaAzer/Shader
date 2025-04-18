using Shader.Data.DTOs;

namespace Shader.Services.Abstraction
{
    public interface IExpenseService
    {
        Task<IEnumerable<RExpenseDTO>> GetExpensesByDateAndTimeRangeAsync(DateOnly? startDate, DateOnly? endDate, TimeOnly? startTime, TimeOnly? endTime);
        Task<IEnumerable<RExpenseDTO>> GetAllExpensesAsync();
        Task<RExpenseDTO> GetExpenseByIdAsync(int id);
        Task<IEnumerable<RExpenseDTO>> GetExpensesByDateAsync(DateOnly date);
        Task<bool> AddExpenseAsync(WExpenseDTO dto);
        Task<bool> UpdateExpenseAsync(int id, WExpenseDTO dto);
        Task<bool> DeleteExpenseAsync(int id);
    }
}
