using Shader.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.ClientTransaction
{
    public class WClientTDto
    {
        public string? Description { get; set; }
        [Required]
        public int ClientId { get; set; }
        public List<WClientTFruitDto> ClientTransactionFruits { get; set; }
    }
}
