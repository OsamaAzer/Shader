using Shader.Data.Entities;

public class ClientTransactionFruit
{
    public int Id { get; set; }
    public int ClientTransactionId { get; set; }
    public ClientTransaction ClientTransaction { get; set; } = null!;
    public int FruitId { get; set; }
    public Fruit Fruit { get; set; } = null!;
    public int NumberOfCagesSold { get; set; }
}
