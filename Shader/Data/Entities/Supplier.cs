namespace Shader.Data.Entities
{
    public class Supplier : BaseEntity
    {
        ICollection<SupplierTransaction> Transactions { get; set; } = new List<SupplierTransaction>();
    }
}
