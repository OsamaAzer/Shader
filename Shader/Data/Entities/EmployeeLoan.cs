namespace Shader.Data.Entities
{
    public class EmployeeLoan
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public MonthlyEmployee Employee { get; set; } = null!;
        public decimal Amount { get; set; } // المبلغ المستلف
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool IsDeleted { get; set; } = false;
    }
}
