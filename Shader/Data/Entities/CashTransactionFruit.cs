using Shader.Data.Entities;

public class CashTransactionFruit
{
    public int CashTransactionId { get; set; }
    public CashTransaction CashTransaction { get; set; } = null!;
    public int FruitId { get; set; }
    public Fruit Fruit { get; set; } = null!;
    public int NumberOfCages { get; set; } 
    public decimal WeightInKilograms { get; set; } 
    public decimal PriceOfKiloGram { get; set; } 
    public decimal TransactionPrice => PriceOfKiloGram * WeightInKilograms; // سعر المعاملة 
}