using Shader.Data.Entities;

public class ClientTransactionFruit
{
    public int ClientTransactionId { get; set; }
    public ClientTransaction ClientTransaction { get; set; } = null!;
    public int FruitId { get; set; }
    public Fruit Fruit { get; set; } = null!;
    public int NumberOfCages { get; set; }
    public decimal WeightInKilograms { get; set; }
    public decimal PriceOfKiloGram { get; set; }
    public decimal TransactionPrice => PriceOfKiloGram * WeightInKilograms;
}
