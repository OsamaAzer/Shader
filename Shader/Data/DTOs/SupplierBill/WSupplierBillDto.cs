using Shader.Data.DTOs.Fruit;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.SupplierBill
{
    public class WSupplierBillDto
    {
        [Required]
        public int SupplierId { get; set; }
        public string? Description { get; set; } = null!;
        [Required]
        public decimal CommissionRate { get; set; }
        [Required]
        public List<FruitWithPriceOfKilogram> Fruits { get; set; } = null!;
    }
}
