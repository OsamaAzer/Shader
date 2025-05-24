
namespace Shader.Data.DTOs.MonthlySalaryRecording
{
    public class RMonthlySRecordingDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal BorrowedAmount { get; set; } // المبلغ المستلف
        public decimal DeductionAmount { get; set; } // المبلغ المخصوم
        public decimal RemainingSalary  => BaseSalary - (BorrowedAmount + DeductionAmount); // الراتب المتبقي
        public DateOnly Date { get; set; } 
        public string SalaryMonth => $"{Date:MMMM} {Date.Year}"; // الشهر والسنة
    }
}
