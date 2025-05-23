namespace Shader.Data.DTOs.MonthlyEmpAbsence
{
    public class RDEmpAbsenceDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public DateTime Date { get; set; }
        public string? Reason { get; set; }
    }
}
