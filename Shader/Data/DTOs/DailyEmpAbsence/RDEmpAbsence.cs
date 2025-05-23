using Shader.Data.Entities;

namespace Shader.Data.DTOs.DailyEmpAbsence
{
    public class RDEmpAbsence
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public string? Reason { get; set; }
    }
}
