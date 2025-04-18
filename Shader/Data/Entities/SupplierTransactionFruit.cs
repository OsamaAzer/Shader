using Shader.Data.Entities;

public class SupplierTransactionFruit
{
    public int Id { get; set; }
    public int SupplierTransactionId { get; set; }
    public SupplierTransaction SupplierTransaction { get; set; } = null!;
    public int FruitId { get; set; }
    public Fruit Fruit { get; set; } = null!;
}
