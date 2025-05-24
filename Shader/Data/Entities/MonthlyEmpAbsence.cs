namespace Shader.Data.Entities
{
    public class MonthlyEmpAbsence
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; } 
        public MonthlyEmployee Employee { get; set; } = null!; 
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now); // تاريخ الغياب
        public bool IsDeleted { get; set; } = false;
    }
}
