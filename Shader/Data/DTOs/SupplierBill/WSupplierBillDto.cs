using Shader.Data.Dtos.Fruit;
using Shader.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Shader.Data.DTOs.SupplierBill
{
    public class WSupplierBillDto
    {
        [Required]
        public int SupplierId { get; set; }
        public string Description { get; set; } = null!;
        [Required]
        public decimal CommissionRate { get; set; }
        public decimal MshalValue { get; set; } 
        public decimal NylonValue { get; set; }
        [Required]
        public List<int> Fruits { get; set; } = null!;
    }
}
