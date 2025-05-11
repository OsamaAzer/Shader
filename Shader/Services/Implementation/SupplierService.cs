using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.Supplier;
using Shader.Data.Entities;
using Shader.Mapping;
using Shader.Services.Abstraction;
using System.Collections.Generic;

namespace Shader.Services.Implementation
{
    public class SupplierService(ShaderContext context) : ISupplierService
    {
        public async Task<IEnumerable<RSupplierDto>> GetAllSuppliersAsync()
        {
            var suppliersDto = context.Suppliers
                .Where(s=> !s.IsDeleted)
                .OrderBy(s => s.Name);
            return await Task.FromResult(suppliersDto.Map<Supplier, RSupplierDto>());
        }
        public async Task<IEnumerable<RSupplierDto>> GetAllSuppliersWithNameAsync(string name)
        {
            var suppliers = await context.Suppliers
                .Where(s => !s.IsDeleted)
                .Where(s => s.Name.ToLower().Contains(name.ToLower()))
                .OrderBy(s => s.Name)
                .ToListAsync();

            return await Task.FromResult(suppliers.Map<Supplier, RSupplierDto>());  
        }
        public async Task<RSupplierDto> GetSupplierByIdAsync(int id)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == id && !s.IsDeleted)
                .FirstOrDefaultAsync();

            if (supplier is null) throw new Exception($"Supplier with id:({id}) does not exist!");
            return supplier.Map<Supplier, RSupplierDto>();
        }
        public async Task<RSupplierDto> AddSupplierAsync(WSupplierDto dto)
        {
            Supplier supplier = dto.Map<WSupplierDto, Supplier>();
            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            return supplier.Map<Supplier, RSupplierDto>();
        }
        public async Task<RSupplierDto> UpdateSupplierAsync(int id, WSupplierDto dto)
        {
            var existingSupplier = await context.Suppliers
                .Where(s => s.Id == id && !s.IsDeleted)
                .FirstOrDefaultAsync();

            if (existingSupplier is null) throw new Exception($"Supplier with id:({id}) does not exist!");
            dto.Map(existingSupplier);
            context.Suppliers.Update(existingSupplier);
            await context.SaveChangesAsync();
            return existingSupplier.Map<Supplier, RSupplierDto>();
        }
        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == id && !s.IsDeleted)
                .FirstOrDefaultAsync();

            if (supplier is null) throw new Exception($"Supplier with id:({id}) does not exist!");
            supplier.IsDeleted = true;
            context.Suppliers.Update(supplier);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
