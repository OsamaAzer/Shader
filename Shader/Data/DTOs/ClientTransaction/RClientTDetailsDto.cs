using Shader.Data.DTOs.ClientTransaction;
using Shader.Data.Entities;

namespace Shader.Data.Dtos.ClientTransaction
{
        public class RClientTDetailsDto 
        {
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public string ClientName { get; set; }
            public decimal Price { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal TotalCageMortgageAmount { get; set; }
            public List<RClientTFruitDto> ClientTransactionFruits { get; set; } 
        }
}
