using System.ComponentModel.DataAnnotations;

namespace Shader.Data.Entities
{
    public class DailyEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [RegularExpression(@"^(?:\+20|0)?(1[0-2]|15)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; }
        public decimal DailySalary { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<DailyEmployeeAbsence> Absenses { get; set; } = new List<DailyEmployeeAbsence>();
    }
}
