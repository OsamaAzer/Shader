using Shader.Data.DTOs.ClientTransaction;
using Shader.Enums;

namespace Shader.Data.DTOs.ShaderTransaction
{
    public class RMerchantTDetailsDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string MerchantName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalCageMortgageAmount { get; set; }
        public List<RMerchantTFruitDto> MerchantTransactionFruits { get; set; }
    }
}
