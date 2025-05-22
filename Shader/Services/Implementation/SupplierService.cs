using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.Supplier;
using Shader.Data.Entities;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;
using System.Collections.Generic;

namespace Shader.Services.Implementation
{
    public class SupplierService(ShaderContext context) : ISupplierService
    {
        public async Task<PagedResponse<RSupplierDto>> GetAllSuppliersAsync(int pageNumber, int pageSize)
        {
            var suppliers = await context.Suppliers
                .Where(s=> !s.IsDeleted)
                .OrderBy(s => s.Name)
                .ToListAsync();
            return suppliers.Map<Supplier, RSupplierDto>().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RSupplierDto>> GetAllMerchantSuppliersAsync(int pageNumber, int pageSize)
        {
            var suppliers = await context.Suppliers
                .Where(s => !s.IsDeleted && s.IsMerchant)
                .OrderBy(s => s.Name)
                .ToListAsync();
            return suppliers.Map<Supplier, RSupplierDto>().CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<RSupplierDto>> GetAllSuppliersWithNameAsync(string name, int pageNumber, int pageSize)
        {
            var suppliers = await context.Suppliers
                .Where(s => !s.IsDeleted)
                .Where(s => s.Name.ToLower().Contains(name.ToLower()))
                .OrderBy(s => s.Name)
                .ToListAsync();

            return suppliers.Map<Supplier, RSupplierDto>().CreatePagedResponse(pageNumber, pageSize);  
        }
        public async Task<RSupplierDto> GetSupplierByIdAsync(int id)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == id && !s.IsDeleted)
                .FirstOrDefaultAsync() ?? 
                throw new Exception($"Supplier with id:({id}) does not exist!");
            return supplier.Map<Supplier, RSupplierDto>();
        }
        public async Task<RSupplierDto> AddSupplierAsync(WSupplierDto dto)
        {
            if (await context.Suppliers.Where(s => !s.IsDeleted).AnyAsync(s => s.Name.ToLower() == dto.Name.ToLower()))
                throw new Exception($"Supplier with name:({dto.Name}) already exists!");
            Supplier supplier = dto.Map<WSupplierDto, Supplier>();
            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            return supplier.Map<Supplier, RSupplierDto>();
        }
        public async Task<RSupplierDto> AddMerchantAsSupplierAsync(int merchantId)
        {
            var existingMerchant = await context.Merchants
                .Where(m =>  !m.IsDeleted)
                .FirstOrDefaultAsync(m => m.Id == merchantId) ??
                throw new Exception($"Merchant with id:({merchantId}) does not exist!");

            var supplier = new Supplier
            {
                Name = existingMerchant.Name,
                City = existingMerchant.City,
                PhoneNumber = existingMerchant.PhoneNumber,
                IsMerchant = true,
                MerchantId = merchantId,
                Merchant = existingMerchant
            };
            await context.Suppliers.AddAsync(supplier);
            await context.SaveChangesAsync();
            return supplier.Map<Supplier, RSupplierDto>();
        }
        public async Task<RSupplierDto> UpdateMerchantAsSupplierAsync(int supplierId, int merchantId)
        {
            var existingSupplier = await context.Suppliers
                .Include(s => s.Merchant)
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == supplierId) ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var existingMerchant = await context.Merchants
                .Where(m => !m.IsDeleted)
                .FirstOrDefaultAsync(m => m.Id == merchantId) ??
                throw new Exception($"Merchant with id:({merchantId}) does not exist!");

            if (await context.Suppliers.Where(s => !s.IsDeleted).AnyAsync( s => s.MerchantId == merchantId))
                throw new Exception($"Merchant with name:({existingMerchant.Name}) already exists as a supplier!");

            existingSupplier.Name = existingMerchant.Name;
            existingSupplier.City = existingMerchant.City;
            existingSupplier.PhoneNumber = existingMerchant.PhoneNumber;
            existingSupplier.MerchantId = existingMerchant.Id;
            existingSupplier.Merchant = existingMerchant;

            context.Suppliers.Update(existingSupplier);
            await context.SaveChangesAsync();
            return existingSupplier.Map<Supplier, RSupplierDto>();
        }
        public async Task<RSupplierDto> UpdateSupplierAsync(int id, WSupplierDto dto)
        {
            var existingSupplier = await context.Suppliers
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == id) ??
                throw new Exception($"Supplier with id:({id}) does not exist!");

            var nameFlag = context.Suppliers
                .Where(s => !s.IsDeleted && s.Name.ToLower() == dto.Name.ToLower())
                .Count() >= 1 && existingSupplier.Name.ToLower() != dto.Name.ToLower();
            if (nameFlag)
                throw new Exception($"Supplier with name:({dto.Name}) already exists!");

            dto.Map(existingSupplier);
            context.Suppliers.Update(existingSupplier);
            await context.SaveChangesAsync();
            return existingSupplier.Map<Supplier, RSupplierDto>();
        }
        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == id && !s.IsDeleted)
                .FirstOrDefaultAsync() ??
                throw new Exception($"Supplier with id:({id}) does not exist!");

            if (supplier.Fruits.Any(f => !f.IsBilled))
                throw new Exception("This supplier still has unbilled fruits!");

            supplier.IsDeleted = true;
            context.Suppliers.Update(supplier);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
