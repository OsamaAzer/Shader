namespace Shader.Data.DTOs.DailySalaryRecording
{
    public class RDailySRecordingDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public decimal DailySalary { get; set; }
        public DateOnly Date { get; set; }
    }
}
