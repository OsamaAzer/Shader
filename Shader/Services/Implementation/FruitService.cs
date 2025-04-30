using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.DTOs;
using Shader.Data.Entities;
using Shader.Enums;
using Shader.Mapping;
using Shader.Services.Abstraction;

namespace Shader.Services.Implementation
{
    public class FruitService(ShaderContext context) : IFruitService
    {
        public async Task<IEnumerable<RAllFruitsDTO>> GetAllFruitsAsync()
        {
            var fruits = context.Fruits.Map<Fruit, RAllFruitsDTO>().ToList();
            return await Task.FromResult<IEnumerable<RAllFruitsDTO>>(fruits);
        }

        public async Task<IEnumerable<RAllFruitsDTO>> GetAllSupplierFruitsAsync(int supplierId)
        {
            var fruits = context.Fruits.Where(f => f.SupplierId == supplierId)
                                    .Map<Fruit, RAllFruitsDTO>().ToList();
            return await Task.FromResult<IEnumerable<RAllFruitsDTO>>(fruits);
        }

        public async Task<RFruitDTO?> GetFruitByIdAsync(int id)
        {
            var fruit = await context.Fruits.FindAsync(id);
            if (fruit is null) return null;
            return fruit.Map<Fruit, RFruitDTO>();
        }

        public async Task<RFruitDTO> AddFruitAsync(AFruitDTO dto)
        {
            var fruit = dto.Map<AFruitDTO, Fruit>();
            fruit.SoldCages = 0;
            fruit.RemainingCages = fruit.TotalCages;
            if (fruit.TotalCages > 0)
                fruit.Status = FruitStatus.InStock;
            await context.Fruits.AddAsync(fruit);
            await context.SaveChangesAsync();
            return fruit.Map<Fruit, RFruitDTO>();
        }

        public async Task<IEnumerable<RFruitDTO>> AddFruitsAsync(int supplierId, List<WRangeFruitDTO> fruitDTOs)
        {
            var fruits = fruitDTOs.Map<WRangeFruitDTO, Fruit>().ToList();
            foreach (var fruit in fruits)
            {
                fruit.SupplierId = supplierId;
                fruit.SoldCages = 0;
                fruit.NumberOfMortgagePaidCages = 0;
                fruit.RemainingCages = fruit.TotalCages;
                if (fruit.TotalCages > 0)
                    fruit.Status = FruitStatus.InStock;
                else
                    fruit.Status = FruitStatus.NotAvailabe;
            }
            await context.Fruits.AddRangeAsync(fruits);
            await context.SaveChangesAsync();
            return fruits.Map<Fruit, RFruitDTO>();
        }

        public async Task<RFruitDTO> UpdateFruitAsync(int id, UFruitDTO dto)
        {
            var fruit = await context.Fruits.FindAsync(id);
            if (fruit is null) return null;

            var supplier = await context.Suppliers.FindAsync(fruit.SupplierId);
            if (supplier is null) return null;

            dto.Map(fruit);
            fruit.SupplierId = supplier.Id;
            fruit.Supplier = supplier;
            fruit.RemainingCages = fruit.TotalCages - fruit.SoldCages;
            if (fruit.RemainingCages > 0)
                fruit.Status = FruitStatus.InStock;
            else
                fruit.Status = FruitStatus.NotAvailabe;

            context.Fruits.Update(fruit);
            await context.SaveChangesAsync();
            return fruit.Map<Fruit, RFruitDTO>();//ToDo NestedObjects
        }

        public async Task<bool> DeleteFruitAsync(int id)
        {
            var fruit = await context.Fruits.FindAsync(id);
            if (fruit is null) return false;
            context.Fruits.Remove(fruit);
            return await context.SaveChangesAsync() > 0;
        }
        
    }
}
