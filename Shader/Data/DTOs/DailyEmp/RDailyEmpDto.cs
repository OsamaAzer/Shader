
namespace Shader.Data.DTOs.DailyEmp
{
    public class RDailyEmpDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public decimal DailySalary { get; set; }
    }
}
