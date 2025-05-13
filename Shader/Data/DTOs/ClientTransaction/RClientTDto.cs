using Shader.Data.DTOs.ClientTransaction;
using Shader.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Dtos.ClientTransaction
{
    public class RClientTDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ClientName { get; set; }
        public decimal TotalAmount { get; set; } // المبلغ الإجمالي
        public decimal TotalCageMortgageAmount { get; set; } // قيمة الرهن
        public List<RClientTFruitDto> ClientTransactionFruits { get; set; }
    }
}
