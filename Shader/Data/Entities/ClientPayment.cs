using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class ClientPayment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal MortgageAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
