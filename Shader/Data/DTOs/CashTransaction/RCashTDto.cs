using Shader.Data.DTOs.CashTransaction;

namespace Shader.Data.Dtos.CashTransaction
{
    public class RCashTDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public List<RCashTFruitDto> CashTransactionFruits { get; set; }
    }
}
