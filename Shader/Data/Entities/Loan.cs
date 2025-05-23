namespace Shader.Data.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public MonthlyEmployee Employee { get; set; } = null!;
        public decimal Amount { get; set; } // المبلغ المستلف
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
