namespace Shader.Data.DTOs.DailyEmpAbsence
{
    public class WDailyPastAbsenceDto
    {
        public int EmployeeId { get; set; } // معرف الموظف
        public DateOnly AbsenceDate { get; set; } // تاريخ الغياب
        public string? Reason { get; set; } = null!; // سبب الغياب
    }
}
