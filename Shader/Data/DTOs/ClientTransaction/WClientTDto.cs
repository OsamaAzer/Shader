using Shader.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.ClientTransaction
{
    public class WClientTDto
    {
        public string? Description { get; set; }
        public decimal DiscountAmount { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public List<WClientTFruitDto> ClientTransactionFruits { get; set; }
    }
}
