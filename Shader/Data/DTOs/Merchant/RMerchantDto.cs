using Shader.Enums;

namespace Shader.Data.DTOs.ShaderSeller
{
    public class RMerchantDto
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public decimal CurrentAmountBalance { get; set; } // Positive = merchant owes us, Negative = we owe merchant
        public decimal CurrentMortgageAmountBalance { get; set; } // Positive = merchant owes us, Negative = we owe merchant
    }
}
