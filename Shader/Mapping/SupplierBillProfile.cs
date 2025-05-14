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
            if (bill.Date == default)
            {
                bill.Date = DateTime.Now;
            }
            if (billDto.SupplierId == 0) throw new Exception("SupplierId cannot be 0");
            bill.SupplierId = billDto.SupplierId;
            bill.NylonValue = billDto.NylonValue;
            bill.MshalValue = billDto.MshalValue;
            bill.CommissionRate = billDto.CommissionRate;
            if (billDto.Fruits.Any() == false || billDto.Fruits.Any(f => f == 0)) 
                throw new Exception("You must select a fruit to be billed!");
            bill.Description = billDto.Description;
            
            return bill;
        }

        public static RSupplierBillDto MapToRSupplierBillDto(this SupplierBill bill)
        {
            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill), "Supplier bill cannot be null");
            }
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
                MshalValue = bill.MshalValue,
                NylonValue = bill.NylonValue,
                Fruits = bill.Fruits.Select(fruit => new RFruitsDto
                {
                    FruitName = fruit.FruitName,
                    TotalCages = fruit.TotalCages,
                    RemainingCages = fruit.RemainingCages,
                    PriceOfKilogramsSold = fruit.PriceOfKilogramsSold,
                    NumberOfKilogramsSold = fruit.NumberOfKilogramsSold,
                    Status = fruit.Status
                }).ToList()
            };
            return billDto;
        }
    }
}
