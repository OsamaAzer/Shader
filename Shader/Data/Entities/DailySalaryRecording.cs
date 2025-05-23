namespace Shader.Data.Entities
{
    public class DailySalaryRecording
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DailyEmployee Employee { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
