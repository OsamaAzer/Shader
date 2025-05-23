namespace Shader.Data.DTOs.DailySalaryRecording
{
    public class RDailySRecordingDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}
