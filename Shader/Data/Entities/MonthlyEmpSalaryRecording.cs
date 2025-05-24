namespace Shader.Data.Entities
{
    public class MonthlyEmpSalaryRecording
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public MonthlyEmployee Employee { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal BorrowedAmount { get; set; } // المبلغ المستلف
        public decimal DeductionAmount { get; set; } // المبلغ المخصوم
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool IsDeleted { get; set; } = false;
    }
}
