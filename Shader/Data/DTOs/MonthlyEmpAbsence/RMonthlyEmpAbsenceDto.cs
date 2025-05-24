namespace Shader.Data.DTOs.MonthlyEmpAbsence
{
    public class RMonthlyEmpAbsenceDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public DateOnly Date { get; set; }
    }
}
