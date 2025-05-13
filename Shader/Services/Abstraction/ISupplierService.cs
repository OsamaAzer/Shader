using Shader.Data.Dtos.Supplier;

namespace Shader.Services.Abstraction
{
    public interface ISupplierService
    {
        Task<RSupplierDto> GetSupplierByIdAsync(int id);
        Task<IEnumerable<RSupplierDto>> GetAllSuppliersAsync();
        Task<IEnumerable<RSupplierDto>> GetAllMerchantSuppliersAsync();
        Task<IEnumerable<RSupplierDto>> GetAllSuppliersWithNameAsync(string name);
        Task<RSupplierDto> AddSupplierAsync(WSupplierDto supplier);
        Task<RSupplierDto> AddMerchantAsSupplierAsync(int merchantId);
        Task<RSupplierDto> UpdateMerchantAsSupplierAsync(int supplierId, int merchantId);
        Task<RSupplierDto> UpdateSupplierAsync(int id, WSupplierDto supplier);
        Task<bool> DeleteSupplierAsync(int id);
        //  todo : supplier bill
    }
}