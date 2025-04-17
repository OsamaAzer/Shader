using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs;
using Shader.Data.Entities;
using Shader.Mapping;
using Shader.Services.Abstraction;
using System.Collections.Generic;

namespace Shader.Services.Implementation
{
    public class SupplierService(ShaderContext context) : ISupplierService
    {
        public async Task<IEnumerable<RSupplierDTO>> GetAllSuppliersAsync()
        {
            var suppliers = context.Suppliers.OrderBy(s => s.Name).ToDTO<Supplier, RSupplierDTO>();
            return suppliers;
        }

        public async Task<RSupplierDTO> GetSupplierByIdAsync(int id)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == id)
                .Select(s => s.ToDTO<Supplier, RSupplierDTO>())
                .FirstOrDefaultAsync();
            if (supplier is null) return await Task.FromResult<RSupplierDTO>(null);
            return supplier;
            //var supplier = await context.Suppliers.FindAsync(id);
            //if (supplier is null) return null;
            //return supplier.ToDTO<Supplier, SupplierDTO>();
        }
        public async Task<bool> AddSupplierAsync(WSupplierDTO dto)
        {
            Supplier supplier = dto.ToEntity<Supplier, WSupplierDTO>();
            await context.Suppliers.AddAsync(supplier);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateSupplierAsync(int id, WSupplierDTO dto)
        {
            var existingSupplier = await context.Suppliers.FindAsync(id);
            if (existingSupplier is null) return false;
            dto.ToEntity(existingSupplier);
            context.Suppliers.Update(existingSupplier);
            return await context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await context.Suppliers.FindAsync(id);
            if(supplier is null) return false;
            context.Suppliers.Remove(supplier);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
