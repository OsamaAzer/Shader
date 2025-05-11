using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.SupplierBill;
using Shader.Data.Entities;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class SupplierBillService(ShaderContext context) : ISupplierBillService
    {
        public async Task<IEnumerable<SupplierBill>> GetAllSupplierBillsAsync()
        {
            return await context.SupplierBills
                .Include(sb => sb.Fruits)
                .Where(sb => !sb.IsDeleted)
                .ToListAsync();
        }
        public async Task<IEnumerable<SupplierBill>> GetSupplierBillsBySupplierIdAsync(int supplierId)
        {
            return await context.SupplierBills
                .Include(sb => sb.Fruits)
                .Where(sb => sb.SupplierId == supplierId && !sb.IsDeleted)
                .ToListAsync();
        }
        public async Task<SupplierBill> GetSupplierBillByIdAsync(int id)
        {
             var bill = await context.SupplierBills
                .Include(sb => sb.Fruits)
                .Where(sb => !sb.IsDeleted)
                .FirstOrDefaultAsync(sb => sb.Id == id) ?? 
                throw new Exception("This supplier bill does not exist!");

            return bill;
        }
        public async Task<RSupplierBillDto> CreateSupplierBillAsync(WSupplierBillDto billDto)
        {
            var bill = billDto.MapToSupplierBill();
            bill.Fruits = context.Fruits
                .Where(f => billDto.Fruits
                .Contains(f.Id)).ToList();
            bill.Price = bill.Fruits.Sum(fruit => fruit.PriceOfKilogramsSold);
            bill.TotalAmount = bill.Price - (bill.NylonValue + bill.MshalValue);
            bill.MyCommisionValue = bill.TotalAmount * (bill.CommissionRate / 100);
            bill.ValueDueToSupplier = bill.TotalAmount - bill.MyCommisionValue;

            foreach (var fruit in bill.Fruits)
            {
                fruit.IsBilled = true;
            }

            bill.Supplier = await context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == billDto.SupplierId) ??
                throw new Exception("This supplier does not exist!");

            await context.SupplierBills.AddAsync(bill);
            await context.SaveChangesAsync();
            var readBillDto = bill.MapToRSupplierBillDto();
            return readBillDto;
        }
        public async Task<RSupplierBillDto> UpdateSupplierBillAsync(int id, WSupplierBillDto billDto)
        {
            var supplierBill = await GetSupplierBillByIdAsync(id) ??
                throw new Exception("This supplier bill does not exist!");
            foreach (var fruit in supplierBill.Fruits)
            {
                if(billDto.Fruits.All(f => f != fruit.Id))
                {
                    fruit.IsBilled = false;
                }
            }
            billDto.Map(supplierBill);
            context.SupplierBills.Update(supplierBill);
            await context.SaveChangesAsync();
            return supplierBill.MapToRSupplierBillDto();
        }
        public async Task<bool> DeleteSupplierBillAsync(int id)
        {
            var supplierBill = await GetSupplierBillByIdAsync(id) ??
                throw new Exception("This supplier bill does not exist!");
            foreach (var fruit in supplierBill.Fruits)
            {
                fruit.IsBilled = false;
            }
            supplierBill.IsDeleted = true;
            context.SupplierBills.Update(supplierBill);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
