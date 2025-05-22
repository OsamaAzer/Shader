using Microsoft.EntityFrameworkCore;
using Shader.Data;
using Shader.Data.Dtos.Fruit;
using Shader.Data.DTOs.SupplierBill;
using Shader.Data.Entities;

namespace Shader.Mapping
{
    public static class SupplierBillProfile
    {
        public static SupplierBill MapToSupplierBill(this WSupplierBillDto billDto, SupplierBill? bill = null)
        {
            bill ??= new SupplierBill();

            if (billDto.Fruits.Any() == false || billDto.Fruits.Any(f => f.FruitId == 0))
                throw new Exception("You must select a fruit to be billed!");

            if (billDto.SupplierId == 0) 
                throw new Exception("SupplierId cannot be 0");

            bill.Date = DateTime.Now;
            bill.SupplierId = billDto.SupplierId;
            bill.CommissionRate = billDto.CommissionRate;
            bill.Description = billDto.Description;
            return bill;
        }

        public static RSupplierBillDto MapToRSupplierBillDto(this SupplierBill bill)
        {
            var billDto = new RSupplierBillDto
            {
                Id = bill.Id,
                SupplierName = bill.Supplier.Name,
                Description = bill.Description,
                Date = bill.Date,
                Price = bill.Price,
                TotalAmount = bill.TotalAmount,
                CommissionRate = bill.CommissionRate,
                MyCommisionValue = bill.MyCommisionValue,
                ValueDueToSupplier = bill.ValueDueToSupplier,
                Fruits = bill.Fruits.Select(fruit => new RFruitsDto
                {
                    FruitName = fruit.FruitName,
                    TotalCages = fruit.TotalCages,
                    NumberOfKilogramsSold = fruit.NumberOfKilogramsSold,
                }).ToList()
            };
            return billDto;
        }
    }
}
