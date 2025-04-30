using Shader.Data.DTOs;

namespace Shader.Services.Abstraction
{
    public interface IExpenseService
    {
        Task<IEnumerable<RExpenseDTO>> GetExpensesByDateAndTimeRangeAsync(DateOnly? startDate, DateOnly? endDate, TimeOnly? startTime, TimeOnly? endTime);
        Task<IEnumerable<RExpenseDTO>> GetExpensesByDateAsync(DateOnly date);
        Task<IEnumerable<RExpenseDTO>> GetAllExpensesAsync();
        Task<RExpenseDTO> GetExpenseByIdAsync(int id);
        Task<RExpenseDTO> AddExpenseAsync(WExpenseDTO dto);
        Task<RExpenseDTO> UpdateExpenseAsync(int id, WExpenseDTO dto);
        Task<bool> DeleteExpenseAsync(int id);
    }
}
