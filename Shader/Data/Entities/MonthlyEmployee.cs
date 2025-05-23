using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class MonthlyEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; } 
        public decimal Salary { get; set; }
        public decimal BorrowedAmount { get; set; }  // المبلغ المستلف
        public decimal RemainingAmount => Salary - BorrowedAmount; // المبلغ المتبقي
        public bool IsDeleted { get; set; } = false;
        public ICollection<Loan> Loans { get; set; } = new List<Loan>(); // القروض
        public ICollection<MonthlyEmployeeAbsence> Absences { get; set; } = new List<MonthlyEmployeeAbsence>(); 
    }
}
