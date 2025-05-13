namespace Shader.Data.DTOs.ClientPayment
{
    public class RClientPaymentDto
    {
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public decimal MortgageAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}
