using Shader.Data.Entities;

namespace Shader.Data.DTOs.DailyEmpAbsence
{
    public class RDailyEmpAbsenceDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public DateOnly Date { get; set; }
    }
}
