namespace Shader.Data.Entities
{
    public class DailyEmpAbsence
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DailyEmployee Employee { get; set; } = null!;
        public DateOnly Date { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
