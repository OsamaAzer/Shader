namespace Shader.Data.DTOs
{
    public class WCashTransactionDTO
    {
        public string? Description { get; set; }
        public List<CashTransactionFruitDTO> CashTransactionFruits { get; set; }
    }
}
