using Shader.Data.DTOs.ClientTransaction;
using Shader.Data.Entities;

namespace Shader.Data.DTOs.ShaderTransaction
{
    public class RMerchantTDto
    {
        public DateTime Date { get; set; }
        public string MerchantName { get; set; }
        public decimal TotalAmount { get; set; } // المبلغ الإجمالي
        public decimal TotalCageMortgageAmount { get; set; } // قيمة الرهن
        public List<RMerchantTFruitDto> MerchantTransactionFruits { get; set; }
    }
}
