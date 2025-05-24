using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class MonthlyEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; } 
        public decimal BaseSalary { get; set; }
        public decimal BorrowedAmount { get; set; }  // المبلغ المستلف
        public decimal RemainingSalary => BaseSalary - BorrowedAmount; // الراتب المتبقي
        public bool IsDeleted { get; set; } = false;
        public ICollection<EmployeeLoan> Loans { get; set; } = new List<EmployeeLoan>(); // القروض
        public ICollection<MonthlyEmpAbsence> Absences { get; set; } = new List<MonthlyEmpAbsence>(); 
    }
}
