using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.DTOs.Employee
{
    public class REmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public SalaryType SalaryType { get; set; } // نوع المرتب
        public decimal Salary { get; set; }
        public decimal BorrowedAmount { get; set; }  // المبلغ المستلف
        public decimal RemainingAmount => Salary - BorrowedAmount; 
    }
}