namespace Shader.Data.Entities
{
    public class MonthlySalaryRecording
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public MonthlyEmployee Employee { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal BorrowedAmount { get; set; } // المبلغ المستلف
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
