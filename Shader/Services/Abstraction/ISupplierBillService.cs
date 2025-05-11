using Shader.Data.DTOs.SupplierBill;
using Shader.Data.Entities;

namespace Shader.Services.Abstraction
{
    public interface ISupplierBillService
    {
        Task<IEnumerable<SupplierBill>> GetAllSupplierBillsAsync();
        Task<IEnumerable<SupplierBill>> GetSupplierBillsBySupplierIdAsync(int supplierId);
        Task<SupplierBill> GetSupplierBillByIdAsync(int id);
        Task<RSupplierBillDto> CreateSupplierBillAsync(WSupplierBillDto supplierBillDto);
        Task<RSupplierBillDto> UpdateSupplierBillAsync(int id, WSupplierBillDto supplierBillDto);
        Task<bool> DeleteSupplierBillAsync(int id);

    }
}
