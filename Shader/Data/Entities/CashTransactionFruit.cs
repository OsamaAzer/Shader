using Shader.Data.Entities;

public class CashTransactionFruit
{
    public int Id { get; set; }
    public int CashTransactionId { get; set; }
    public CashTransaction CashTransaction { get; set; }
    public int FruitId { get; set; }
    public Fruit Fruit { get; set; }
    public int NumberOfCages { get; set; } // عدد القفص
}