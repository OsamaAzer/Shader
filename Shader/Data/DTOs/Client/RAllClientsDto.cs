using Shader.Enums;

namespace Shader.Data.Dtos.Client
{
    public class RAllClientsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string? PhoneNumber { get; set; }
        public ClientStatus Status { get; set; }
        public decimal TotalRemainingAmount { get; set; }
        public decimal TotalRemainingMortgageAmount { get; set; } // قيمة الرهن المتبقي
    }
}
