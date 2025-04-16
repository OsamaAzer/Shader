using Shader.Data.Entities;

public class CageDetails
{
    public int Id { get; set; }
    public decimal? CageMortgageValue { get; set; } // قيمة رهن القفص
    public int? NumberOfMortgagePaidCages { get; set; } // عدد القفص المدفوع الرهن
    public int TotalCages { get; set; }
    public int SoldCages { get; set; }
    public int RemainingCages { get; set; }
    public int ReturnedCages { get; set; }
    public int UnReturnedCages { get; set; }
}