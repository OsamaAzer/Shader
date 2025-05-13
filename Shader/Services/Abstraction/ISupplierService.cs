using Shader.Data.Dtos.Supplier;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface ISupplierService
    {
        Task<PagedResponse<RSupplierDto>> GetAllSuppliersAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RSupplierDto>> GetAllMerchantSuppliersAsync(int pageNumber, int pageSize);
        Task<PagedResponse<RSupplierDto>> GetAllSuppliersWithNameAsync(string name, int pageNumber, int pageSize);
        Task<RSupplierDto> GetSupplierByIdAsync(int id);
        Task<RSupplierDto> AddSupplierAsync(WSupplierDto supplier);
        Task<RSupplierDto> AddMerchantAsSupplierAsync(int merchantId);
        Task<RSupplierDto> UpdateMerchantAsSupplierAsync(int supplierId, int merchantId);
        Task<RSupplierDto> UpdateSupplierAsync(int id, WSupplierDto supplier);
        Task<bool> DeleteSupplierAsync(int id);
        //  todo : supplier bill
    }
}