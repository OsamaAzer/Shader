namespace Shader.Data.Entities
{
    public class MonthlyEmployeeAbsence
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; } 
        public MonthlyEmployee Employee { get; set; } = null!; 
        public DateTime Date { get; set; }
        public string? Reason { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
