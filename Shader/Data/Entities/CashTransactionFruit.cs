using Shader.Data.Entities;

public class CashTransactionFruit
{
    public int CashTransactionId { get; set; }
    public CashTransaction CashTransaction { get; set; }
    public int FruitId { get; set; }
    public Fruit Fruit { get; set; }
    public int NumberOfCages { get; set; } 
    public decimal WeightInKilograms { get; set; }
    public decimal PriceOfKiloGram { get; set; }
    public decimal TransactionPrice { get; set; } 
}