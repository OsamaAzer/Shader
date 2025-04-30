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
            var suppliersDTO = context.Suppliers.OrderBy(s => s.Name).Map<Supplier, RSupplierDTO>().ToList();
            return await Task.FromResult<IEnumerable<RSupplierDTO>>(suppliersDTO);
        }

        public async Task<RSupplierDTO> GetSupplierByIdAsync(int id)
        {
            var supplier = await context.Suppliers.FindAsync(id);
            if (supplier is null) return await Task.FromResult<RSupplierDTO>(null);
            return supplier.Map<Supplier, RSupplierDTO>();
            //var supplier = await context.Suppliers.FindAsync(id);
            //if (supplier is null) return null;
            //return supplier.ToDTO<Supplier, SupplierDTO>();
        }
        public async Task<RSupplierDTO> AddSupplierAsync(WSupplierDTO dto)
        {
            Supplier supplier = dto.Map<WSupplierDTO, Supplier>();
            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            return supplier.Map<Supplier, RSupplierDTO>();
        }
        public async Task<RSupplierDTO> UpdateSupplierAsync(int id, WSupplierDTO dto)
        {
            var existingSupplier = await context.Suppliers.FindAsync(id);
            if (existingSupplier is null) return null;
            dto.Map(existingSupplier);
            context.Suppliers.Update(existingSupplier);
            await context.SaveChangesAsync();
            return existingSupplier.Map<Supplier, RSupplierDTO>();
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
