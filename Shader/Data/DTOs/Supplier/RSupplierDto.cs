namespace Shader.Data.Dtos.Supplier
{
    public class RSupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsMerchant { get; set; }  // هل المورد تاجر
        public decimal TotalAmountOfBills { get; set; } // المبلغ الإجمالي للفواتير
    }
}
