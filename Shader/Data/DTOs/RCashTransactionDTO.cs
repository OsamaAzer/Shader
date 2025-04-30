namespace Shader.Data.DTOs
{
    public class RCashTransactionDTO
    {
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public List<CashTransactionFruitDTO> CashTransactionFruits { get; set; }
    }
}
