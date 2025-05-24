using Shader.Data.Entities;

namespace Shader.Data.DTOs.Loan
{
    public class RLoanDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public decimal Amount { get; set; } // المبلغ المستلف
        public DateOnly Date { get; set; }
    }
}
