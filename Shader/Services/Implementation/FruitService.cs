using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.Fruit;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class FruitService(ShaderContext context) : IFruitService
    {
        public async Task<IEnumerable<RFruitDto>> GetAllFruitsAsync()
        {
            var fruits = await context.Fruits
                .Where(f => !f.IsDeleted)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitDto>());
        }
        public async Task<IEnumerable<Fruit>> GetSupplierFruitsToBeBilledAsync(int supplierId)
        {
            var supplier = await context.Suppliers
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(s => s.Id == supplierId) ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = await context.Fruits
                .Where(f => f.SupplierId == supplierId)
                .Where(f => !f.IsBilled && !f.IsDeleted)
                .Where(f => f.Status == FruitStatus.NotAvailabe)
                .ToListAsync();
            return fruits;
        }
        public async Task<IEnumerable<RFruitDto>> GetInStockFruitsAsync()
        {
            var fruits = await context.Fruits
                .Where(f => f.Status == FruitStatus.InStock)
                .Where(f => !f.IsDeleted)
                .OrderByDescending(f => f.RemainingCages)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitDto>());
        }
        public async Task<IEnumerable<RFruitDto>> GetInStockSupplierFruitsAsync(int supplierId)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == supplierId && !s.IsDeleted)
                .FirstOrDefaultAsync() ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = await context.Fruits
                .Where(f => f.SupplierId == supplierId)
                .Where(f => f.Status == FruitStatus.InStock)
                .Where (f => !f.IsDeleted)
                .OrderByDescending(f => f.RemainingCages)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitDto>());
        }
        public async Task<IEnumerable<RFruitDto>> GetUnAvailableFruitsAsync()
        {
            var fruits = await context.Fruits
                .Where(f => f.Status == FruitStatus.NotAvailabe)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitDto>());
        }
        public async Task<IEnumerable<RFruitDto>> GetAllSupplierFruitsAsync(int supplierId)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == supplierId && !s.IsDeleted)
                .FirstOrDefaultAsync() ??
                throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = await context.Fruits
                .Where(f => f.SupplierId == supplierId)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
            return await Task.FromResult(fruits.Map<Fruit, RFruitDto>());
        }
        public async Task<IEnumerable<RFruitDto>> SearchWithFruitNameAsync(string fruitName)
        {
            var fruits = await context.Fruits
                .Where(f => f.FruitName.ToLower().Contains(fruitName.ToLower()))
                .Where(f => !f.IsDeleted)
                .ToListAsync();

            return await Task.FromResult(fruits.Map<Fruit, RFruitDto>());
        }
        public async Task<RFruitDto> GetFruitByIdAsync(int id)
        {
            var fruit = await context.Fruits
                .Where(f => f.Id == id && !f.IsDeleted)
                .FirstOrDefaultAsync() ??
                throw new Exception($"Fruit with id:({id}) does not exist!");

            return fruit.Map<Fruit, RFruitDto>();
        }
        public async Task<RFruitDto> AddFruitCagesAsync(int id, int numberOfCages)
        {
            var fruit = await context.Fruits
                .Where(s => s.Id == id && !s.IsDeleted)
                .FirstOrDefaultAsync() ?? throw new Exception($"Fruit with id:({id}) does not exist!");

            if (numberOfCages <= 0) throw new Exception("Number of cages must be greater than 0!");

            fruit.TotalCages += numberOfCages;
            fruit.RemainingCages += numberOfCages;

            if (fruit.TotalCages > 0 && fruit.RemainingCages > 0)
                fruit.Status = FruitStatus.InStock;
            else
                fruit.Status = FruitStatus.NotAvailabe;

            context.Fruits.Update(fruit);
            await context.SaveChangesAsync();
            return fruit.Map<Fruit, RFruitDto>();
        }
        public async Task<IEnumerable<RFruitDto>> AddFruitsAsync(int supplierId, List<WRangeFruitDto> fruitDtos)
        {
            var supplier = await context.Suppliers
                .Where(s => s.Id == supplierId && !s.IsDeleted)
                .FirstOrDefaultAsync();
            if (supplier == null) throw new Exception($"Supplier with id:({supplierId}) does not exist!");

            var fruits = fruitDtos.Map<WRangeFruitDto, Fruit>().ToList();
            foreach (var fruit in fruits)
            {
                fruit.Date = DateTime.Now;
                fruit.SupplierId = supplierId;
                fruit.SoldCages = 0;
                fruit.RemainingCages = fruit.TotalCages;

                if (fruit.TotalCages > 0 && fruit.RemainingCages > 0)
                    fruit.Status = FruitStatus.InStock;
                else
                    fruit.Status = FruitStatus.NotAvailabe;
            }
            await context.Fruits.AddRangeAsync(fruits);
            await context.SaveChangesAsync();
            return fruits.Map<Fruit, RFruitDto>();
        }
        public async Task<RFruitDto> UpdateFruitAsync(int id, UFruitDto dto)
        {
            var fruit = await context.Fruits
                .Where(f => !f.IsDeleted)
                .FirstOrDefaultAsync(f => f.Id == id) ?? 
                throw new Exception($"Fruit with id:({id}) does not exist!");

            var supplier = await context.Suppliers
                .Where(f => !f.IsDeleted)
                .FirstOrDefaultAsync(f => f.Id == fruit.SupplierId) ?? 
                throw new Exception($"Supplier with id:({fruit.SupplierId}) does not exist!");

            dto.Map(fruit);
            fruit.SupplierId = supplier.Id;
            fruit.Supplier = supplier;
            fruit.RemainingCages = fruit.TotalCages - fruit.SoldCages;

            if (fruit.RemainingCages > 0 && fruit.RemainingCages > 0)
                fruit.Status = FruitStatus.InStock;
            else
                fruit.Status = FruitStatus.NotAvailabe;

            // todo : check if the fruit has a bill or not
            // todo : check the transactions mortgage values have been done on the fruit if the mortgage amount changed or unchecked!

            context.Fruits.Update(fruit);
            await context.SaveChangesAsync();
            return fruit.Map<Fruit, RFruitDto>();//ToDo NestedObjects
        }
        public async Task<bool> DeleteFruitAsync(int id)
        {
            var fruit = await context.Fruits.FindAsync(id);
            if (fruit is null) throw new Exception($"Fruit with id:({id}) does not exist!");
            fruit.Status = FruitStatus.NotAvailabe;
            fruit.IsDeleted = true;
            context.Fruits.Update(fruit);
            return await context.SaveChangesAsync() > 0;
        }
        
    }
}
