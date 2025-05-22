using Shader.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; } 
        public SalaryType SalaryType { get; set; } // نوع المرتب
        public decimal Salary { get; set; }
        public decimal BorrowedAmount { get; set; }  // المبلغ المستلف
        public decimal RemainingAmount => Salary - BorrowedAmount; // المبلغ المتبقي
        public bool IsDeleted { get; set; } = false;
        public ICollection<Absence> Absences { get; set; } = new List<Absence>(); 
    }
}
