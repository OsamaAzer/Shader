using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.CashTransaction
{
    public class WCashTDto
    {
        public string? Description { get; set; }
        [Required]
        public List<WCashTFruitDto> CashTransactionFruits { get; set; }
    }
}
