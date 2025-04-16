namespace Shader.Data.Entities
{
    public class SupplierTransaction : Transaction
    {
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
