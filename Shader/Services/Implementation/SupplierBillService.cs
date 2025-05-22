using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs.SupplierBill;
using Shader.Data.Entities;
using Shader.Helpers;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class SupplierBillService(ShaderContext context) : ISupplierBillService
    {
        public async Task<PagedResponse<SupplierBill>> GetAllSupplierBillsAsync(int pageNumber, int pageSize)
        {
            var bills =  await context.SupplierBills
                .Include(sb => sb.Fruits)
                .Where(sb => !sb.IsDeleted)
                .OrderByDescending(sb => sb.Date)
                .ToListAsync();
            return bills.CreatePagedResponse(pageNumber, pageSize);
        }
        public async Task<PagedResponse<SupplierBill>> GetSupplierBillsBySupplierIdAsync(int supplierId, int pageNumber, int pageSize)
        {
            var bills = await context.SupplierBills
                .Include(sb => sb.Fruits)
                .Where(sb => sb.SupplierId == supplierId && !sb.IsDeleted)
                .OrderByDescending(sb => sb.Date)
                .ToListAsync();
            return bills.CreatePagedResponse(pageNumber, pageSize);
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
            var supplier = await context.Suppliers
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == billDto.SupplierId) ??
                throw new Exception("This supplier does not exist!");

            if (billDto.SupplierId == 0) 
                throw new Exception("SupplierId cannot be 0");
            if (billDto.Fruits == null || billDto.Fruits.Count == 0 || billDto.Fruits.Any(f => f.FruitId == 0))
                throw new Exception("You must select a fruit to be billed!");
            if (billDto.CommissionRate <= 0)
                throw new Exception("Commission rate should be greater than Zero!!");

            var bill = billDto.MapToSupplierBill();
            //bill.Fruits = await context.Fruits
            //    .Where(f => billDto.Fruits
            //    .Select(fruit => fruit.FruitId)
            //    .Contains(f.Id) && !f.IsDeleted)
            //    .ToListAsync();

            foreach (var fruitDto in billDto.Fruits)
            {
                var fruit = await context.Fruits
                    .Where(f => f.Id == fruitDto.FruitId && !f.IsDeleted)
                    .FirstOrDefaultAsync() ?? throw new Exception($"This fruit {fruitDto.FruitId} does not exist!");

                if (fruit.IsBilled)
                    throw new Exception($"This fruit {fruit.FruitName} has already been billed!"); 

                fruit.PriceOfKilogramInBill = fruitDto.PriceOfKilogram;
                fruit.IsBilled = true;
                bill.Fruits.Add(fruit);
            }
            bill.Price = bill.Fruits.Select(f => f.NumberOfKilogramsSold * f.PriceOfKilogramInBill).Sum();
            bill.TotalAmount = bill.Price - bill.Fruits.Select(f => f.MashalValue + f.NylonValue).Sum();
            bill.MyCommisionValue = bill.TotalAmount * (bill.CommissionRate / 100);
            bill.ValueDueToSupplier = bill.TotalAmount - bill.MyCommisionValue;

            supplier.TotalAmountOfBills += bill.ValueDueToSupplier;
            supplier.TotalRemainingAmount = supplier.TotalAmountOfBills - supplier.TotalAmountPaid;
            context.Suppliers.Update(supplier);
            await context.SupplierBills.AddAsync(bill);
            await context.SaveChangesAsync();
            return bill.MapToRSupplierBillDto();
        }
        public async Task<RSupplierBillDto> UpdateSupplierBillAsync(int id, WSupplierBillDto billDto)
        {
            if (billDto.SupplierId == 0)
                throw new Exception("SupplierId cannot be 0");
            if (billDto.Fruits == null || billDto.Fruits.Count == 0 || billDto.Fruits.Any(f => f.FruitId == 0))
                throw new Exception("You must select a fruit to be billed!");
            if (billDto.CommissionRate <= 0)
                throw new Exception("Commission rate should be greater than Zero!!");

            var bill = await GetSupplierBillByIdAsync(id) ??
                throw new Exception("This supplier bill does not exist!");

            var supplier = await context.Suppliers
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == billDto.SupplierId) ??
                throw new Exception("This supplier does not exist!");
            supplier.TotalAmountOfBills -= bill.TotalAmount;

            foreach (var fruit in bill.Fruits)
            {
                if(billDto.Fruits.All(f => f.FruitId != fruit.Id))
                {
                    fruit.IsBilled = false;
                    bill.TotalAmount -= fruit.PriceOfKilogramInBill * fruit.NumberOfKilogramsSold - (fruit.MashalValue + fruit.NylonValue);
                    fruit.PriceOfKilogramInBill = 0;

                }
            }
            billDto.Map(bill);
            supplier.TotalAmountOfBills += bill.TotalAmount;
            context.Suppliers.Update(supplier);
            context.SupplierBills.Update(bill);
            await context.SaveChangesAsync();
            return bill.MapToRSupplierBillDto();
        }
        public async Task<bool> DeleteSupplierBillAsync(int id)
        {
            var supplierBill = await GetSupplierBillByIdAsync(id) ??
                throw new Exception("This supplier bill does not exist!");

            var supplier = await context.Suppliers
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == supplierBill.SupplierId) ??
                throw new Exception("This supplier does not exist!");
            supplier.TotalAmountOfBills -= supplierBill.TotalAmount;

            foreach (var fruit in supplierBill.Fruits)
            {
                fruit.IsBilled = false;
            }
            supplierBill.IsDeleted = true;
            context.Suppliers.Update(supplier);
            context.SupplierBills.Update(supplierBill);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
