
namespace Shader.Data.DTOs.MonthlySalaryRecording
{
    public class RMonthlySRecordingDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = null!;
        public decimal BaseSalary { get; set; }
        public decimal BorrowedAmount { get; set; } // المبلغ المستلف
        public decimal RemainingSalary  => BaseSalary - BorrowedAmount; // الراتب المتبقي
        public DateTime Date { get; set; } 
    }
}
