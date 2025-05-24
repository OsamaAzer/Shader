namespace Shader.Data.Entities
{
    public class DailyEmpSalaryRecording
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DailyEmployee Employee { get; set; } = null!;
        public decimal DailySalary { get; set; } 
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool IsDeleted { get; set; } = false;
    }
}
