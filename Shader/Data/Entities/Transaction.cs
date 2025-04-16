using Shader.Enums;

namespace Shader.Data.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        ICollection<Fruit> Fruits { get; set; } = new List<Fruit>();
    }
}
