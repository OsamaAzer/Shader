using Shader.Data.Dtos.Fruit;
using Shader.Data.Entities;

namespace Shader.Data.DTOs.SupplierBill
{
    public class WSupplierBillDto
    {
        public int SupplierId { get; set; }
        public string Description { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal MshalValue { get; set; } 
        public decimal NylonValue { get; set; } 
        public List<int> Fruits { get; set; }
    }
}
