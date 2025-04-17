using Shader.Data.DTOs;

namespace Shader.Services.Abstraction
{
    public interface ISupplierService
    {
        Task<RSupplierDTO> GetSupplierByIdAsync(int id);
        Task<IEnumerable<RSupplierDTO>> GetAllSuppliersAsync();
        Task<bool> AddSupplierAsync(WSupplierDTO supplier);
        Task<bool> UpdateSupplierAsync(int id, WSupplierDTO supplier);
        Task<bool> DeleteSupplierAsync(int id);
    }
}