using Shader.Data.DTOs.SupplierBill;
using Shader.Data.Entities;
using Shader.Helpers;

namespace Shader.Services.Abstraction
{
    public interface ISupplierBillService
    {
        Task<PagedResponse<SupplierBill>> GetAllSupplierBillsAsync(int pageNumber, int pageSize);
        Task<PagedResponse<SupplierBill>> GetSupplierBillsBySupplierIdAsync(int supplierId, int pageNumber, int pageSize);
        Task<SupplierBill> GetSupplierBillByIdAsync(int id);
        Task<RSupplierBillDto> CreateSupplierBillAsync(WSupplierBillDto supplierBillDto);
        Task<RSupplierBillDto> UpdateSupplierBillAsync(int id, WSupplierBillDto supplierBillDto);
        Task<bool> DeleteSupplierBillAsync(int id);

    }
}
