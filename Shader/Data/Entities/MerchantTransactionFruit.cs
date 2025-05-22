namespace Shader.Data.Entities
{
    public class MerchantTransactionFruit
    {
        public int MerchantTransactionId { get; set; }
        public MerchantTransaction MerchantTransaction { get; set; }
        public int FruitId { get; set; }
        public Fruit Fruit { get; set; } 
        public int NumberOfCages { get; set; }
        public decimal WeightInKilograms { get; set; }
        public decimal PriceOfKiloGram { get; set; }
        public decimal TransactionPrice { get; set; }
    }
}
