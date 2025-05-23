
namespace Shader.Data.DTOs.MonthlyEmp
{
    public class RMonthlyEmpDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public decimal Salary { get; set; }
        public decimal BorrowedAmount { get; set; }  // المبلغ المستلف
        public decimal RemainingAmount => Salary - BorrowedAmount; 
    }
}