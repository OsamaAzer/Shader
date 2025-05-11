namespace Shader.Data.Entities
{
    public class ClientPayments
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal MortgageAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public bool IsDeleted { get; set; }
    }
}
